using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.CmTr.TransactionForm;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;

namespace Architecture.BLL.CmTr.TransactionForm;

public class CommTradeTransactionHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader obj)
	{
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		List<GetItemGlIdsandItemName> dtItemGl = new List<GetItemGlIdsandItemName>();
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
			vh.BillAmount = Conversion.ToDouble(obj.BillAmount);
			vh.VoucherAmount = Conversion.ToDouble(obj.BillAmount);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = 0;
			vh.ManualBillNo = Conversion.ToString(obj.DocNo);
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.CommTradeTransactionDetailList.Count > 0)
			{
				string CustomerRemarks = string.Empty;
				string SupplierRemarks = string.Empty;
				double TotalCustomerAmount = 0.0;
				double TotalSupplierAmount = 0.0;
				double GainAndLoss = 0.0;
				foreach (CommTradeTransactionDetail item5 in obj.CommTradeTransactionDetailList.ToList())
				{
					if (item5.ActionTypeId != 3)
					{
						if (dtSupplier.Count <= 0)
						{
							throw new Exception("CustomerGlId Record Not found");
						}
						List<SupplierCustomerGlAccountId> find4 = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item5.CustomerId)).ToList();
						List<SupplierCustomerGlAccountId> findSupplier = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item5.SupplierId)).ToList();
						if (findSupplier.Count == 0)
						{
							throw new Exception("SupplierGlId Record Not found");
						}
						if (find4.Count > 0)
						{
							vh.RefAccountId = Conversion.ToInt(find4[0].GlAccountId);
							CustomerRemarks = "Item: " + item5.ItemName + " Qty: " + item5.Qty + " Weight: " + item5.CusNetWeight + " Rate: " + item5.CusRate + " Item Amount: " + item5.CusAmount;
							VoucherDetail vd10 = new VoucherDetail();
							vd10.AccountId = Conversion.ToInt(find4[0].GlAccountId);
							vd10.AgainstAccountId = Conversion.ToInt(findSupplier[0].GlAccountId);
							vd10.Comments = Conversion.ToString(CustomerRemarks);
							vd10.DebitAmount = Conversion.ToDouble(item5.CusAmount);
							vd10.ItemId = Conversion.ToInt(item5.ItemId);
							vd10.QtyOut = Conversion.ToDouble(item5.Qty);
							vd10.WeightOut = Conversion.ToDouble(item5.CusNetWeight);
							vd10.ItemRate = Conversion.ToDouble(item5.CusRate);
							vd10.ItemAmount = Conversion.ToDouble(item5.CusAmount);
							vd10.DMultiCurrencyId = Conversion.ToInt(item5.CurrencyId);
							vd10.DExchangeCurrencyRate = Conversion.ToDouble(item5.ExchangeRate);
							vd10.DCurrencyAmount = Conversion.ToDouble(item5.CustFcyAmount);
							vh.voucherDetailList.Add(vd10);
							SupplierRemarks = "Item: " + item5.ItemName + " Qty: " + item5.Qty + " Weight: " + item5.SupNetWeight + " Rate: " + item5.SupRate + " Item Amount: " + item5.SupAmount;
							VoucherDetail vd16 = new VoucherDetail();
							vd16.AccountId = Conversion.ToInt(findSupplier[0].GlAccountId);
							vd16.AgainstAccountId = Conversion.ToInt(find4[0].GlAccountId);
							vd16.Comments = Conversion.ToString(SupplierRemarks);
							vd16.CreditAmount = Conversion.ToDouble(item5.SupAmount);
							vd16.ItemId = Conversion.ToInt(item5.ItemId);
							vd16.QtyIn = Conversion.ToDouble(item5.Qty);
							vd16.WeightIn = Conversion.ToDouble(item5.SupNetWeight);
							vd16.ItemRate = Conversion.ToDouble(item5.SupRate);
							vd16.ItemAmount = Conversion.ToDouble(item5.SupAmount);
							vd16.DMultiCurrencyId = Conversion.ToInt(item5.CurrencyId);
							vd16.DExchangeCurrencyRate = Conversion.ToDouble(item5.ExchangeRate);
							vd16.DCurrencyAmount = Conversion.ToDouble(item5.SupFcyAmount);
							vh.voucherDetailList.Add(vd16);
							TotalCustomerAmount += vd10.DebitAmount;
							TotalSupplierAmount += vd16.CreditAmount;
						}
					}
				}
				if (obj.ActionId != 3)
				{
					GainAndLoss = TotalCustomerAmount - TotalSupplierAmount;
					if (GainAndLoss != 0.0)
					{
						string GainAndLossRemarks = " CustomerAmount " + TotalCustomerAmount + " SuppliersAmount " + TotalSupplierAmount + " DiffAmount " + Math.Abs(GainAndLoss);
						if (GainAndLoss < 0.0)
						{
							VoucherDetail vd9 = new VoucherDetail();
							vd9.AccountId = Conversion.ToInt(obj.DifferenceAccountId);
							vd9.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd9.Comments = Conversion.ToString(GainAndLossRemarks);
							vd9.DebitAmount = Math.Abs(Conversion.ToDouble(GainAndLoss));
							vd9.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
							vd9.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
							vd9.DCurrencyAmount = Math.Abs(Conversion.ToDouble(GainAndLoss)) / Conversion.ToDouble(obj.ExchangeRate);
							vh.voucherDetailList.Add(vd9);
						}
						if (GainAndLoss > 0.0)
						{
							VoucherDetail vd8 = new VoucherDetail();
							vd8.AccountId = Conversion.ToInt(obj.DifferenceAccountId);
							vd8.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd8.Comments = Conversion.ToString(GainAndLossRemarks);
							vd8.CreditAmount = Conversion.ToDouble(GainAndLoss);
							vd8.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
							vd8.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
							vd8.DCurrencyAmount = Conversion.ToDouble(GainAndLoss) / Conversion.ToDouble(obj.ExchangeRate);
							vh.voucherDetailList.Add(vd8);
						}
					}
				}
			}
			if (obj.ActionId != 3)
			{
				if (obj.CommTradeTransactionCommBrokeryList.Count > 0)
				{
					int GLAccountIdForSubBrokery = 0;
					string Remarks4 = string.Empty;
					foreach (CommTradeTransactionCommBrokery item3 in obj.CommTradeTransactionCommBrokeryList.ToList())
					{
						Remarks4 = "";
						if (item3.RevenueExpenseId == 1 && item3.Amount > 0m)
						{
							GLAccountIdForSubBrokery = item3.CommissionAgentGLId;
							Remarks4 = ((item3.ChargesIncentiveId != 5) ? ("@BrokeryRate " + item3.Rate) : ("@CommRate " + item3.Rate));
							VoucherDetail vd7 = new VoucherDetail();
							vd7.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd7.AgainstAccountId = Conversion.ToInt(item3.CommissionAgentGLId);
							vd7.Comments = Conversion.ToString(Remarks4);
							vd7.DebitAmount = Conversion.ToDouble(item3.Amount);
							vd7.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
							vd7.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
							vd7.DCurrencyAmount = Conversion.ToDouble(item3.Amount) / Conversion.ToDouble(obj.ExchangeRate);
							vh.voucherDetailList.Add(vd7);
							VoucherDetail vd15 = new VoucherDetail();
							vd15.AccountId = Conversion.ToInt(item3.CommissionAgentGLId);
							vd15.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd15.Comments = Conversion.ToString(Remarks4);
							vd15.CreditAmount = Conversion.ToDouble(item3.Amount);
							vd15.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
							vd15.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
							vd15.DCurrencyAmount = Conversion.ToDouble(item3.Amount) / Conversion.ToDouble(obj.ExchangeRate);
							vh.voucherDetailList.Add(vd15);
						}
						if (item3.RevenueExpenseId == 2 && item3.Amount > 0m)
						{
							List<SupplierCustomerGlAccountId> find3 = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item3.CommissionAgentId)).ToList();
							if (find3.Count == 0)
							{
								throw new Exception("SubBroker GLAccountId not Found");
							}
							Remarks4 = "@SubBrokeryRate " + item3.Rate;
							VoucherDetail vd6 = new VoucherDetail();
							vd6.AccountId = Conversion.ToInt(GLAccountIdForSubBrokery);
							vd6.AgainstAccountId = Conversion.ToInt(find3[0].GlAccountId);
							vd6.Comments = Conversion.ToString(Remarks4);
							vd6.DebitAmount = Conversion.ToDouble(item3.Amount);
							vd6.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
							vd6.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
							vd6.DCurrencyAmount = Conversion.ToDouble(item3.Amount) / Conversion.ToDouble(obj.ExchangeRate);
							vh.voucherDetailList.Add(vd6);
							VoucherDetail vd14 = new VoucherDetail();
							vd14.AccountId = Conversion.ToInt(find3[0].GlAccountId);
							vd14.AgainstAccountId = Conversion.ToInt(GLAccountIdForSubBrokery);
							vd14.Comments = Conversion.ToString(Remarks4);
							vd14.CreditAmount = Conversion.ToDouble(item3.Amount);
							vd14.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
							vd14.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
							vd14.DCurrencyAmount = Conversion.ToDouble(item3.Amount) / Conversion.ToDouble(obj.ExchangeRate);
							vh.voucherDetailList.Add(vd14);
						}
					}
				}
				if (obj.CommTradeTransactionPmOtherList.Count > 0)
				{
					dtItemGl = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
					string Remarks3 = string.Empty;
					foreach (CommTradeTransactionPmOther item2 in obj.CommTradeTransactionPmOtherList.ToList())
					{
						int ItemCreditAccountId = 0;
						if (item2.CreditAccountId > 0)
						{
							ItemCreditAccountId = item2.CreditAccountId;
						}
						else if (dtItemGl.Count > 0)
						{
							List<GetItemGlIdsandItemName> find2 = dtItemGl.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count > 0)
							{
								ItemCreditAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
							}
						}
						if (ItemCreditAccountId == 0)
						{
							throw new Exception("CreditAccountId Not Found against Packing Material");
						}
						if (item2.Qty > 0m)
						{
							Remarks3 = "Qty: " + item2.Qty;
						}
						if (item2.ItemSize != null && item2.ItemSize != string.Empty && item2.ItemSize != "0")
						{
							Remarks3 = Remarks3 + " Size: " + item2.ItemSize;
						}
						if (item2.ItemWeight > 0m)
						{
							Remarks3 = Remarks3 + " Weight: " + item2.ItemWeight;
						}
						if (item2.Rate > 0m)
						{
							Remarks3 = Remarks3 + " @Rate: " + item2.Rate;
						}
						if (item2.Amount > 0m)
						{
							Remarks3 = Remarks3 + " ItemAmount: " + item2.Amount;
						}
						VoucherDetail vd5 = new VoucherDetail();
						vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd5.AgainstAccountId = Conversion.ToInt(ItemCreditAccountId);
						vd5.Comments = Conversion.ToString(Remarks3);
						vd5.DebitAmount = Conversion.ToDouble(item2.Amount);
						vd5.ItemId = Conversion.ToInt(item2.ItemId);
						vd5.QtyOut = Conversion.ToDouble(item2.Qty);
						vd5.WeightOut = Conversion.ToDouble(item2.ItemWeight);
						vd5.ItemRate = Conversion.ToDouble(item2.Rate);
						vd5.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd5.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd5.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						vd5.DCurrencyAmount = Conversion.ToDouble(item2.Amount) / Conversion.ToDouble(obj.ExchangeRate);
						vh.voucherDetailList.Add(vd5);
						VoucherDetail vd13 = new VoucherDetail();
						vd13.AccountId = Conversion.ToInt(ItemCreditAccountId);
						vd13.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd13.Comments = Conversion.ToString(Remarks3);
						vd13.CreditAmount = Conversion.ToDouble(item2.Amount);
						vd13.ItemId = Conversion.ToInt(item2.ItemId);
						vd13.QtyOut = Conversion.ToDouble(item2.Qty);
						vd13.WeightOut = Conversion.ToDouble(item2.ItemWeight);
						vd13.ItemRate = Conversion.ToDouble(item2.Rate);
						vd13.ItemAmount = Conversion.ToDouble(item2.Amount);
						vd13.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd13.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						vd13.DCurrencyAmount = Conversion.ToDouble(item2.Amount) / Conversion.ToDouble(obj.ExchangeRate);
						vh.voucherDetailList.Add(vd13);
					}
				}
				if (obj.CommTradeTransactionRevExpList.Count > 0)
				{
					string Remarks2 = string.Empty;
					foreach (CommTradeTransactionRevExp item in obj.CommTradeTransactionRevExpList.ToList())
					{
						int SupplierGlId = 0;
						if (item.SupplierCustomerId > 0)
						{
							List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(item.SupplierCustomerId)).ToList();
							if (find.Count > 0)
							{
								SupplierGlId = Conversion.ToInt(find[0].GlAccountId);
							}
						}
						else
						{
							DataTable dt = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(0, item.AccountId);
							if (dt.Rows.Count > 0)
							{
								SupplierGlId = Conversion.ToInt(dt.Rows[0]["GLAccountId"]);
							}
						}
						if (SupplierGlId == 0)
						{
							throw new Exception("SupplierGlId not Found");
						}
						if (item.Qty > 0m)
						{
							Remarks2 = "Qty: " + item.Qty;
						}
						if (item.Rate > 0m)
						{
							Remarks2 = Remarks2 + " Rate: " + item.Rate;
						}
						Remarks2 = ((!(item.CreditAmt > 0m)) ? (Remarks2 + " Amount: " + item.DebitAmt) : (Remarks2 + " Amount: " + item.CreditAmt));
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(SupplierGlId);
						vd4.Comments = Conversion.ToString(Remarks2);
						vd4.DebitAmount = Conversion.ToDouble(item.CreditAmt);
						vd4.CreditAmount = Conversion.ToDouble(item.DebitAmt);
						vd4.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd4.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						if (item.CreditAmt > 0m)
						{
							vd4.DCurrencyAmount = Conversion.ToDouble(item.CreditAmt) / Conversion.ToDouble(obj.ExchangeRate);
						}
						else
						{
							vd4.DCurrencyAmount = Conversion.ToDouble(item.DebitAmt) / Conversion.ToDouble(obj.ExchangeRate);
						}
						vh.voucherDetailList.Add(vd4);
						VoucherDetail vd12 = new VoucherDetail();
						vd12.AccountId = Conversion.ToInt(SupplierGlId);
						vd12.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd12.Comments = Conversion.ToString(Remarks2);
						vd12.CreditAmount = Conversion.ToDouble(item.CreditAmt);
						vd12.DebitAmount = Conversion.ToDouble(item.DebitAmt);
						vd12.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd12.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						vd12.DCurrencyAmount = vd4.DCurrencyAmount;
						vh.voucherDetailList.Add(vd12);
					}
				}
				if (obj.CommTradeTransactionVehicleFreightDetailList.Count > 0)
				{
					string Remarks = string.Empty;
					foreach (CommTradeTransactionVehicleFreightDetail item4 in obj.CommTradeTransactionVehicleFreightDetailList.ToList())
					{
						if (item4.Remarks == null || item4.Remarks == string.Empty)
						{
							if (item4.VehicleQty > 0m)
							{
								Remarks = "Vehicle Qty: " + item4.VehicleQty;
							}
							if (item4.VehicleNo != null && item4.VehicleNo != string.Empty)
							{
								Remarks = Remarks + "Vehicle #: " + item4.VehicleNo.ToString();
							}
							if (item4.BiltyNo != null && item4.BiltyNo != string.Empty)
							{
								Remarks = Remarks + " Bilty #: " + item4.BiltyNo.ToString();
							}
							if (item4.BiltyFreight > 0m)
							{
								Remarks = Remarks + " Bilty Freight: " + item4.BiltyFreight;
							}
							if (item4.NetPaid > 0m)
							{
								Remarks = Remarks + " Net Paid: " + item4.NetPaid;
							}
						}
						else
						{
							Remarks = item4.Remarks;
						}
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd3.AgainstAccountId = Conversion.ToInt(item4.CreditAccountId);
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.DebitAmount = Conversion.ToDouble(item4.NetPaid);
						vd3.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd3.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						if (item4.NetPaid > 0m && obj.ExchangeRate > 0m)
						{
							vd3.DCurrencyAmount = Conversion.ToDouble(item4.NetPaid) / Conversion.ToDouble(obj.ExchangeRate);
						}
						vh.voucherDetailList.Add(vd3);
						VoucherDetail vd11 = new VoucherDetail();
						vd11.AccountId = Conversion.ToInt(item4.CreditAccountId);
						vd11.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd11.Comments = Conversion.ToString(Remarks);
						vd11.CreditAmount = Conversion.ToDouble(item4.NetPaid);
						vd11.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd11.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						vd11.DCurrencyAmount = vd3.DCurrencyAmount;
						vh.voucherDetailList.Add(vd11);
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

	public static int Save(Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.CmTr.TransactionForm.CommTradeTransactionHeader().SetData(obj, "[CmTr].[USP_CommTradeTransactionHeader_Insert]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.CmTr.TransactionForm.CommTradeTransactionHeader().SetData(obj, "[CmTr].[USP_CommTradeTransactionHeader_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader ReadById(int Id)
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
			new List<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>();
			return new Architecture.DAL.CmTr.TransactionForm.CommTradeTransactionHeader().GetData("[CmTr].[USP_CommTradeTransaction_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryDetail(int Id)
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
				Value = "FormHistoryDetail"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransaction_GetAllMethod]", p);
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransaction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransaction_GetAllMethod]", p);
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
			GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransaction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommissionTransactionSlipAndRegister(ReportsParameters obj)
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
			if (obj.CustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerId",
					Value = obj.CustomerId
				});
			}
			if (obj.SupplierId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierId",
					Value = obj.SupplierId
				});
			}
			if (obj.CommissionAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionAgentId",
					Value = obj.CommissionAgentId
				});
			}
			if (obj.SubBrokerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SubBrokerId",
					Value = obj.SubBrokerId
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.LanguageId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LanguageId",
					Value = obj.LanguageId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransactionHeader_Register]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommTradeTransactionCustomerExpense_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CommTradeTransactionHeaderId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransactionCustomerExpense_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommTradeTransactionPM_SubReport(int Id, int LanguageId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CommTradeTransactionHeaderId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@LanguageId",
				Value = LanguageId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransactionPM_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommTradeTransactionSupplierSummary_SubReport(int Id, int LanguageId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@CommTradeTransactionHeaderId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@LanguageId",
				Value = LanguageId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.CmTr.TransactionForm.CommTradeTransactionHeader>("[CmTr].[USP_CommTradeTransactionSupplierSummary_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromCommTransaction(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("[CmTr].[USP_GetDataForDropDownFromCommTradeTransaction]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLastSavedCommBrokeryAndDiffAccounts(int OrganizationId, int CompanyId)
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
				ParameterName = "@Activity",
				Value = "GetLastSavedCommBrokeryAndDiffAccounts"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("[CmTr].[USP_CommTradeTransaction_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
