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

public class InvCommAgentTradeBill
{
	public static VoucherHead MakeVoucher(Architecture.Model.Inventory.InvCommAgentTradeBill obj)
	{
		List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> dtSupplier = new List<Architecture.Model.Inventory.SupplierCustomerGlAccountId>();
		new DataTable();
		VoucherHead vh = new VoucherHead();
		try
		{
			int CustomerGlId = 0;
			int CommissionAgentGlId = 0;
			int CustCommissionAgentGlId = 0;
			int BrokerAgentGlId = 0;
			int CustBrokerAgentGlId = 0;
			double TradingTotalDebit = 0.0;
			double TradingTotalCredit = 0.0;
			string TradingRemarks = ((!string.IsNullOrEmpty(obj.RemarksHeader)) ? (obj.RemarksHeader + " ") : "");
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
			vh.ManualBillNo = Conversion.ToString(obj.DocNo);
			vh.DueDate = DateTime.Now;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EnteryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count <= 0)
			{
				throw new Exception("SupplierCustomer list not Found");
			}
			List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> find = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.SupplierId)).ToList();
			if (find.Count <= 0)
			{
				throw new Exception("Supplier GLAccountId not Found");
			}
			vh.RefAccountId = Conversion.ToInt(find[0].GlAccountId);
			List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findCus2 = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.CustomerId)).ToList();
			if (findCus2.Count <= 0)
			{
				throw new Exception("Customer GLAccountId not Found");
			}
			CustomerGlId = Conversion.ToInt(findCus2[0].GlAccountId);
			if (obj.CommissionAgentId > 0)
			{
				List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findComm2 = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.CommissionAgentId)).ToList();
				if (findComm2.Count <= 0)
				{
					throw new Exception("Commission Agent GLAccountId not Found");
				}
				CommissionAgentGlId = Conversion.ToInt(findComm2[0].GlAccountId);
			}
			if (obj.BrokerId > 0)
			{
				List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findbroker = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.BrokerId)).ToList();
				if (findbroker.Count <= 0)
				{
					throw new Exception("Broker Agent GLAccountId not Found");
				}
				BrokerAgentGlId = Conversion.ToInt(findbroker[0].GlAccountId);
			}
			if (obj.CustCommissionAgentId > 0)
			{
				List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findCustComm = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.CustCommissionAgentId)).ToList();
				if (findCustComm.Count <= 0)
				{
					throw new Exception("Commission Agent GLAccountId not Found");
				}
				CustCommissionAgentGlId = Conversion.ToInt(findCustComm[0].GlAccountId);
			}
			if (obj.CustBrokerId > 0)
			{
				List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findCustbroker = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.CustBrokerId)).ToList();
				if (findCustbroker.Count <= 0)
				{
					throw new Exception("Broker Agent GLAccountId not Found");
				}
				CustBrokerAgentGlId = Conversion.ToInt(findCustbroker[0].GlAccountId);
			}
			if (vh.RefAccountId == 0)
			{
				throw new Exception("SupplierGlId Not Found");
			}
			if (CustomerGlId == 0)
			{
				throw new Exception("CustomerGlId Not Found");
			}
			if (obj.TradingGlAccountId == 0)
			{
				throw new Exception("TradingGlAccountId Not Found");
			}
			if (obj.InvCommAgentTradeBillDetailslist.Count > 0)
			{
				List<string> vehicleNos = (from x in obj.InvCommAgentTradeBillDetailslist?.Where((InvCommAgentTradeBillDetail x) => !string.IsNullOrWhiteSpace(x.VehicleNo))
					select x.VehicleNo.Trim()).Distinct().ToList() ?? new List<string>();
				_ = string.Empty;
				double TotalAmount = 0.0;
				foreach (InvCommAgentTradeBillDetail item7 in obj.InvCommAgentTradeBillDetailslist.ToList())
				{
					if (item7.ActionTypeId != 3)
					{
						if (item7.ItemAmount > 0.0 && item7.RatePurchase > 0m)
						{
							_ = $"RefNo: {item7.RefDocNo}, Item: {item7.ItemName}, Qty: {item7.Qty}, Weight: {item7.BillWeight}, Rate: {item7.RatePurchase}, Item Amount: {item7.ItemAmount}";
							TradingRemarks += $"Purchase : RefNo: {item7.RefDocNo}, Item: {item7.ItemName}, Qty: {item7.Qty}, Weight: {item7.BillWeight}, Rate: {item7.RatePurchase}, VehicleNo: {item7.VehicleNo}{Environment.NewLine}";
							TradingTotalDebit += Conversion.ToDouble(item7.ItemAmount);
							VoucherDetail vd28 = new VoucherDetail();
							vd28.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd28.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
							vd28.Comments = BuildSupplierRemarks(obj, item7);
							vd28.RefInvoiceNo = Conversion.ToString(item7.RefDocNo);
							vd28.CreditAmount = Conversion.ToDouble(item7.ItemAmount);
							vd28.ItemId = Conversion.ToInt(item7.ItemId);
							vd28.QtyIn = Conversion.ToDouble(item7.Qty);
							vd28.ItemRate = Conversion.ToDouble(item7.RatePurchase);
							vd28.WeightIn = Conversion.ToDouble(item7.BillWeight);
							vd28.ItemAmount = Conversion.ToDouble(item7.ItemAmount);
							vd28.VehicleNo = Conversion.ToString(item7.VehicleNo);
							vh.voucherDetailList.Add(vd28);
							TotalAmount += vd28.CreditAmount;
						}
						if (item7.SaleNetAmount > 0.0 && item7.RateSale > 0m)
						{
							_ = $"RefNo: {item7.RefDocNo}, Item: {item7.ItemName}, Qty: {item7.Qty}, Weight: {item7.SaleBillWeight}, Rate: {item7.RateSale}, Item Amount: {item7.SaleAmount}";
							TradingRemarks += $"Sales : {item7.ItemName} Qty: {item7.Qty}, Weight: {item7.SaleBillWeight}, Rate: {item7.RateSale}{Environment.NewLine}";
							VoucherDetail vd29 = new VoucherDetail();
							vd29.AccountId = Conversion.ToInt(CustomerGlId);
							vd29.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
							vd29.Comments = BuildCustomerRemarks(obj, item7);
							vd29.RefInvoiceNo = Conversion.ToString(item7.RefDocNo);
							vd29.DebitAmount = Conversion.ToDouble(item7.SaleAmount);
							vd29.ItemId = Conversion.ToInt(item7.ItemId);
							vd29.QtyOut = Conversion.ToDouble(item7.Qty);
							vd29.ItemRate = Conversion.ToDouble(item7.RateSale);
							vd29.WeightOut = Conversion.ToDouble(item7.SaleBillWeight);
							vd29.ItemAmount = Conversion.ToDouble(item7.SaleAmount);
							vd29.VehicleNo = Conversion.ToString(item7.VehicleNo);
							vh.voucherDetailList.Add(vd29);
							TradingTotalCredit += Conversion.ToDouble(item7.SaleAmount);
						}
					}
				}
				if (obj.InvCommAgentTradePurchaseExpList.Count > 0)
				{
					foreach (CommisionAgentBillPurchaseExpense item6 in obj.InvCommAgentTradePurchaseExpList.ToList())
					{
						if (item6.Amount > 0.0 && item6.InvExpItemId > 0)
						{
							TradingRemarks = TradingRemarks + "Purchase Expenses : " + item6.Remarks + Environment.NewLine;
							TradingTotalDebit += Conversion.ToDouble(item6.Amount);
							string Comments2 = item6.Remarks;
							if (vehicleNos.Any())
							{
								string label4 = ((vehicleNos.Count == 1) ? "Vehicle No" : "Vehicle Nos");
								Comments2 = Comments2 + " Against " + label4 + ": " + string.Join(", ", vehicleNos);
							}
							VoucherDetail vd27 = new VoucherDetail();
							vd27.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd27.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
							vd27.Comments = Comments2;
							vd27.CreditAmount = Conversion.ToDouble(item6.Amount);
							vd27.ItemId = Conversion.ToInt(item6.InvExpItemId);
							vd27.QtyIn = Conversion.ToDouble(item6.Qty);
							vd27.ItemRate = Conversion.ToDouble(item6.Rate);
							vd27.ItemAmount = Conversion.ToDouble(item6.Amount);
							vh.voucherDetailList.Add(vd27);
						}
					}
				}
				if (obj.InvCommAgentTradeSaleExpList.Count > 0)
				{
					foreach (CommisionAgentBillSaleExpense item5 in obj.InvCommAgentTradeSaleExpList.ToList())
					{
						if (item5.Amount > 0.0 && item5.InvExpItemId > 0)
						{
							string Comments = item5.Remarks;
							if (vehicleNos.Any())
							{
								string label3 = ((vehicleNos.Count == 1) ? "Vehicle No" : "Vehicle Nos");
								Comments = Comments + " Against " + label3 + ": " + string.Join(", ", vehicleNos);
							}
							VoucherDetail vd15 = new VoucherDetail();
							vd15.AccountId = Conversion.ToInt(CustomerGlId);
							vd15.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
							vd15.Comments = Comments;
							vd15.DebitAmount = Conversion.ToDouble(item5.Amount);
							vd15.ItemId = Conversion.ToInt(item5.InvExpItemId);
							vd15.QtyOut = Conversion.ToDouble(item5.Qty);
							vd15.ItemRate = Conversion.ToDouble(item5.Rate);
							vd15.ItemAmount = Conversion.ToDouble(item5.Amount);
							vh.voucherDetailList.Add(vd15);
							TradingTotalCredit += Conversion.ToDouble(item5.Amount);
							TradingRemarks = TradingRemarks + "Sales Expenses : " + item5.Remarks + Environment.NewLine;
						}
					}
				}
				if (obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList != null && obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Count > 0)
				{
					foreach (CommisionAgentBillSaleExpenseCreditToReleventAc item4 in obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.ToList())
					{
						if (!(item4.Amount > 0.0) || item4.AccountId <= 0)
						{
							continue;
						}
						if (obj.DeliveryTermSale == "Load" || obj.DeliveryTermSale == "Load & PartyWeight" || obj.DeliveryTermSale == "Load & FactoryWeight")
						{
							if (CustomerGlId == item4.AccountId)
							{
								throw new Exception("Sale Expenses Credit Account and Customer Account cannot be same because delivery term is load.");
							}
							VoucherDetail vd14 = new VoucherDetail();
							vd14.AccountId = Conversion.ToInt(CustomerGlId);
							vd14.AgainstAccountId = Conversion.ToInt(item4.AccountId);
							vd14.Comments = Conversion.ToString(item4.Remarks);
							vd14.DebitAmount = Conversion.ToDouble(item4.Amount);
							vh.voucherDetailList.Add(vd14);
							if (obj.TradingGlAccountId == item4.AccountId && obj.DeliveryTermSale == "Load")
							{
								TradingRemarks = TradingRemarks + "Sales Packing Material : " + item4.Remarks + Environment.NewLine;
								TradingTotalCredit += Conversion.ToDouble(item4.Amount);
								continue;
							}
							VoucherDetail vd25 = new VoucherDetail();
							vd25.AccountId = Conversion.ToInt(item4.AccountId);
							vd25.AgainstAccountId = Conversion.ToInt(CustomerGlId);
							vd25.Comments = Conversion.ToString(item4.Remarks);
							vd25.CreditAmount = Conversion.ToDouble(item4.Amount);
							vh.voucherDetailList.Add(vd25);
						}
						else
						{
							TradingRemarks = TradingRemarks + "Sales Packing Material : " + item4.Remarks + Environment.NewLine;
							TradingTotalDebit += Conversion.ToDouble(item4.Amount);
							VoucherDetail vd26 = new VoucherDetail();
							vd26.AccountId = Conversion.ToInt(item4.AccountId);
							vd26.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
							vd26.Comments = Conversion.ToString(item4.Remarks);
							vd26.CreditAmount = Conversion.ToDouble(item4.Amount);
							vh.voucherDetailList.Add(vd26);
						}
					}
				}
				if (obj.InvCommAgentTradeFreightExpList != null && obj.InvCommAgentTradeFreightExpList.Count > 0)
				{
					foreach (CommisionAgentBillPurchaseFreightExpense item3 in obj.InvCommAgentTradeFreightExpList.ToList())
					{
						if (!(item3.Amount > 0m) || item3.AccountId <= 0)
						{
							continue;
						}
						if (obj.DeliveryTerm == "Load" || obj.DeliveryTerm == "Load & PartyWeight" || obj.DeliveryTerm == "Load & FactoryWeight")
						{
							TradingRemarks = TradingRemarks + "Purchase Freight Expenses : " + item3.Remarks + Environment.NewLine;
							TradingTotalDebit += Conversion.ToDouble(item3.Amount);
							VoucherDetail vd23 = new VoucherDetail();
							vd23.AccountId = Conversion.ToInt(item3.AccountId);
							vd23.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
							vd23.Comments = Conversion.ToString(item3.Remarks);
							vd23.CreditAmount = Conversion.ToDouble(item3.Amount);
							vh.voucherDetailList.Add(vd23);
							continue;
						}
						if (vh.RefAccountId == item3.AccountId)
						{
							throw new Exception("Freight Credit Account and Supplier Account cannot be same because delivery term is ponch.");
						}
						VoucherDetail vd13 = new VoucherDetail();
						vd13.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd13.AgainstAccountId = Conversion.ToInt(item3.AccountId);
						vd13.Comments = Conversion.ToString(item3.Remarks);
						vd13.DebitAmount = Conversion.ToDouble(item3.Amount);
						vh.voucherDetailList.Add(vd13);
						if (obj.TradingGlAccountId != Conversion.ToInt(item3.AccountId))
						{
							VoucherDetail vd24 = new VoucherDetail();
							vd24.AccountId = Conversion.ToInt(item3.AccountId);
							vd24.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd24.Comments = Conversion.ToString(item3.Remarks);
							vd24.CreditAmount = Conversion.ToDouble(item3.Amount);
							vh.voucherDetailList.Add(vd24);
						}
						else
						{
							TradingRemarks = TradingRemarks + "Purchase Freight Expenses : " + item3.Remarks + Environment.NewLine;
							TradingTotalCredit += Conversion.ToDouble(item3.Amount);
						}
					}
				}
				foreach (CommisionAgentBillFreightDetail item2 in obj.CommisionAgentBillFreightDetailList)
				{
					if (item2.Amount <= 0m || item2.AccountId <= 0)
					{
						continue;
					}
					int GlAccountId = 0;
					List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findCus = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item2.AccountId)).ToList();
					if (findCus.Count > 0)
					{
						GlAccountId = Conversion.ToInt(findCus[0].GlAccountId);
						HashSet<int> Ponchds = new HashSet<int> { 2, 5, 6 };
						if (item2.EntrySideId == 1)
						{
							if (Ponchds.Contains(obj.DeliveryTermPurchaseId))
							{
								TradingRemarks = TradingRemarks + "Purchase Freight : " + item2.Remarks + Environment.NewLine;
								TradingTotalCredit += Conversion.ToDouble(item2.NetAmount);
								vh.voucherDetailList.Add(new VoucherDetail
								{
									AccountId = GlAccountId,
									AgainstAccountId = obj.TradingGlAccountId,
									DebitAmount = Conversion.ToDouble(item2.NetAmount),
									Comments = BuildFreightRemarks(obj, item2)
								});
							}
						}
						else if (item2.EntrySideId == 2 && Ponchds.Contains(obj.DeliveryTermSaleId))
						{
							TradingRemarks = TradingRemarks + "Sale Freight : " + item2.Remarks + Environment.NewLine;
							TradingTotalDebit += Conversion.ToDouble(item2.NetAmount);
							vh.voucherDetailList.Add(new VoucherDetail
							{
								AccountId = GlAccountId,
								AgainstAccountId = obj.TradingGlAccountId,
								CreditAmount = Conversion.ToDouble(item2.NetAmount),
								Comments = BuildFreightRemarks(obj, item2)
							});
						}
						continue;
					}
					throw new Exception(((item2.EntrySideId == 1) ? "Purchase" : "Sale") + " Freight GLAccountId not Found");
				}
				if (obj.CommAmount > 0.0 && obj.CommissionAgentId > 0)
				{
					if (obj.TradingGlAccountId == obj.CommissionDebitGLId || obj.CommissionDebitGLId == 0)
					{
						TradingRemarks += $"Purchase Commission : % {obj.CommRate} : {obj.CommAmount}{Environment.NewLine}";
						TradingTotalDebit += Conversion.ToDouble(obj.CommAmount);
					}
					else
					{
						VoucherDetail vd12 = new VoucherDetail();
						vd12.AccountId = Conversion.ToInt(obj.CommissionDebitGLId);
						vd12.AgainstAccountId = Conversion.ToInt(CommissionAgentGlId);
						vd12.Comments = "% " + obj.CommRate + " " + obj.CommAmount;
						vd12.DebitAmount = Conversion.ToDouble(obj.CommAmount);
						vh.voucherDetailList.Add(vd12);
					}
					if (obj.TradingGlAccountId == CommissionAgentGlId)
					{
						TradingRemarks += $"Purchase Commission : % {obj.CommRate} : {obj.CommAmount}{Environment.NewLine}";
						TradingTotalCredit += Conversion.ToDouble(obj.CommAmount);
					}
					else
					{
						VoucherDetail vd22 = new VoucherDetail();
						vd22.AccountId = Conversion.ToInt(CommissionAgentGlId);
						vd22.AgainstAccountId = Conversion.ToInt(obj.CommissionDebitGLId);
						vd22.Comments = "% " + obj.CommRate + " " + obj.CommAmount;
						vd22.CreditAmount = Conversion.ToDouble(obj.CommAmount);
						vh.voucherDetailList.Add(vd22);
					}
				}
				if (obj.BrokeryAmount > 0.0 && obj.BrokerId > 0)
				{
					if (obj.TradingGlAccountId == obj.BrokeryDebitGLId || obj.BrokeryDebitGLId == 0)
					{
						TradingRemarks += $"Purchase Brokery : % {obj.BrokeryRate} : {obj.BrokeryAmount}{Environment.NewLine}";
						TradingTotalDebit += Conversion.ToDouble(obj.BrokeryAmount);
					}
					else
					{
						VoucherDetail vd11 = new VoucherDetail();
						vd11.AccountId = Conversion.ToInt(obj.BrokeryDebitGLId);
						vd11.AgainstAccountId = Conversion.ToInt(BrokerAgentGlId);
						vd11.Comments = "% " + obj.BrokeryRate + " " + obj.BrokeryAmount;
						vd11.DebitAmount = Conversion.ToDouble(obj.BrokeryAmount);
						vh.voucherDetailList.Add(vd11);
					}
					if (obj.TradingGlAccountId == BrokerAgentGlId)
					{
						TradingRemarks += $"Purchase Brokery : % {obj.BrokeryRate} : {obj.BrokeryAmount}{Environment.NewLine}";
						TradingTotalCredit += Conversion.ToDouble(obj.BrokeryAmount);
					}
					else
					{
						VoucherDetail vd21 = new VoucherDetail();
						vd21.AccountId = Conversion.ToInt(BrokerAgentGlId);
						vd21.AgainstAccountId = Conversion.ToInt(obj.BrokeryDebitGLId);
						vd21.Comments = "% " + obj.BrokeryRate + " " + obj.BrokeryAmount;
						vd21.CreditAmount = Conversion.ToDouble(obj.BrokeryAmount);
						vh.voucherDetailList.Add(vd21);
					}
				}
				if (obj.CustCommAmount > 0.0 && obj.CustCommissionAgentId > 0)
				{
					if (obj.TradingGlAccountId == obj.CustCommissionDebitGLId || obj.CustCommissionDebitGLId == 0)
					{
						TradingRemarks += $"Sales Commission : % {obj.CustCommRate} : {obj.CustCommAmount}{Environment.NewLine}";
						TradingTotalDebit += Conversion.ToDouble(obj.CustCommAmount);
					}
					else
					{
						VoucherDetail vd10 = new VoucherDetail();
						vd10.AccountId = Conversion.ToInt(obj.CustCommissionDebitGLId);
						vd10.AgainstAccountId = Conversion.ToInt(CustCommissionAgentGlId);
						vd10.Comments = "% " + obj.CustCommRate + " " + obj.CustCommAmount;
						vd10.DebitAmount = Conversion.ToDouble(obj.CustCommAmount);
						vh.voucherDetailList.Add(vd10);
					}
					if (obj.TradingGlAccountId == CustCommissionAgentGlId)
					{
						TradingRemarks += $"Sales Commission : % {obj.CustCommRate} : {obj.CustCommAmount}{Environment.NewLine}";
						TradingTotalCredit += Conversion.ToDouble(obj.CustCommAmount);
					}
					else
					{
						VoucherDetail vd20 = new VoucherDetail();
						vd20.AccountId = Conversion.ToInt(CustCommissionAgentGlId);
						vd20.AgainstAccountId = Conversion.ToInt(obj.CustCommissionDebitGLId);
						vd20.Comments = "% " + obj.CustCommRate + " " + obj.CustCommAmount;
						vd20.CreditAmount = Conversion.ToDouble(obj.CustCommAmount);
						vh.voucherDetailList.Add(vd20);
					}
				}
				if (obj.CustBrokeryAmount > 0.0 && obj.CustBrokerId > 0)
				{
					if (obj.TradingGlAccountId == obj.CustBrokeryDebitGLId || obj.CustBrokeryDebitGLId == 0)
					{
						TradingRemarks += $"Sales Brokery : % {obj.CustBrokeryRate} : {obj.CustBrokeryAmount}{Environment.NewLine}";
						TradingTotalDebit += Conversion.ToDouble(obj.CustBrokeryAmount);
					}
					else
					{
						VoucherDetail vd9 = new VoucherDetail();
						vd9.AccountId = Conversion.ToInt(obj.CustBrokeryDebitGLId);
						vd9.AgainstAccountId = Conversion.ToInt(CustBrokerAgentGlId);
						vd9.Comments = "% " + obj.CustBrokeryRate + " " + obj.CustBrokeryAmount;
						vd9.DebitAmount = Conversion.ToDouble(obj.CustBrokeryAmount);
						vh.voucherDetailList.Add(vd9);
					}
					if (obj.TradingGlAccountId == CustBrokerAgentGlId)
					{
						TradingRemarks += $"Sales Brokery : % {obj.CustBrokeryRate} : {obj.CustBrokeryAmount}{Environment.NewLine}";
						TradingTotalCredit += Conversion.ToDouble(obj.CustBrokeryAmount);
					}
					else
					{
						VoucherDetail vd19 = new VoucherDetail();
						vd19.AccountId = Conversion.ToInt(CustBrokerAgentGlId);
						vd19.AgainstAccountId = Conversion.ToInt(obj.CustBrokeryDebitGLId);
						vd19.Comments = "% " + obj.CustBrokeryRate + " " + obj.CustBrokeryAmount;
						vd19.CreditAmount = Conversion.ToDouble(obj.CustBrokeryAmount);
						vh.voucherDetailList.Add(vd19);
					}
				}
				if (obj.CommisionAgentBillCommissionDetailList.Count > 0)
				{
					foreach (CommisionAgentBillCommissionDetail item in obj.CommisionAgentBillCommissionDetailList.ToList())
					{
						if (!(item.commissionAmount > 0m) || item.commissionAgentId <= 0)
						{
							continue;
						}
						CommissionAgentGlId = 0;
						List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> findComm = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.commissionAgentId)).ToList();
						if (findComm.Count > 0)
						{
							CommissionAgentGlId = Conversion.ToInt(findComm[0].GlAccountId);
							if (obj.TradingGlAccountId == item.debitAccountId || item.debitAccountId == 0)
							{
								TradingRemarks += item.commissionRemarks;
								TradingTotalDebit += Conversion.ToDouble(item.commissionAmount);
							}
							else
							{
								VoucherDetail vd8 = new VoucherDetail();
								vd8.AccountId = Conversion.ToInt(item.debitAccountId);
								vd8.AgainstAccountId = Conversion.ToInt(CommissionAgentGlId);
								vd8.Comments = item.commissionRemarks;
								vd8.DebitAmount = Conversion.ToDouble(item.commissionAmount);
								vh.voucherDetailList.Add(vd8);
							}
							if (obj.TradingGlAccountId == CommissionAgentGlId)
							{
								TradingRemarks += item.commissionRemarks;
								TradingTotalCredit += Conversion.ToDouble(item.commissionAmount);
								continue;
							}
							VoucherDetail vd18 = new VoucherDetail();
							vd18.AccountId = Conversion.ToInt(CommissionAgentGlId);
							vd18.AgainstAccountId = Conversion.ToInt(item.debitAccountId);
							vd18.Comments = item.commissionRemarks;
							vd18.CreditAmount = Conversion.ToDouble(item.commissionAmount);
							vh.voucherDetailList.Add(vd18);
							continue;
						}
						if (item.agentTypeId == 1)
						{
							throw new Exception("Commission Agent GlAccountId Not Found");
						}
						throw new Exception("Broker Agent GlAccountId Not Found");
					}
				}
				decimal PurchaseTax = obj.InvCommAgentTradeBillDetailslist?.Sum((InvCommAgentTradeBillDetail x) => x.TaxAmountPurchase) ?? 0m;
				if (PurchaseTax > 0m)
				{
					string taxremarks2 = string.Join(", ", (from x in obj.InvCommAgentTradeBillDetailslist?.Where((InvCommAgentTradeBillDetail x) => x.TaxAmountPurchase > 0m)
						select $"{x.TaxPercentPurchase}% {x.TaxNamePurchase}").Distinct() ?? Enumerable.Empty<string>());
					if (obj.PurchaseTaxAccountId == 0)
					{
						throw new Exception("Purchase TaxAccount field is required");
					}
					VoucherDetail vd7 = new VoucherDetail();
					vd7.AccountId = obj.PurchaseTaxAccountId;
					vd7.AgainstAccountId = vh.RefAccountId;
					vd7.Comments = taxremarks2;
					vd7.DebitAmount = Conversion.ToDouble(PurchaseTax);
					vh.voucherDetailList.Add(vd7);
					VoucherDetail vd17 = new VoucherDetail();
					vd17.AccountId = vh.RefAccountId;
					vd17.AgainstAccountId = obj.PurchaseTaxAccountId;
					vd17.Comments = taxremarks2;
					vd17.CreditAmount = Conversion.ToDouble(PurchaseTax);
					vh.voucherDetailList.Add(vd17);
				}
				decimal SalesTax = obj.InvCommAgentTradeBillDetailslist?.Sum((InvCommAgentTradeBillDetail x) => x.TaxAmountSale) ?? 0m;
				if (SalesTax > 0m)
				{
					string taxremarks = string.Join(", ", (from x in obj.InvCommAgentTradeBillDetailslist?.Where((InvCommAgentTradeBillDetail x) => x.TaxAmountSale > 0m)
						select $"{x.TaxPercentSale}% {x.TaxNameSale}").Distinct() ?? Enumerable.Empty<string>());
					if (obj.SaleTaxAccountId == 0)
					{
						throw new Exception("Sales TaxAccount field is required");
					}
					VoucherDetail vd6 = new VoucherDetail();
					vd6.AccountId = CustomerGlId;
					vd6.AgainstAccountId = obj.SaleTaxAccountId;
					vd6.Comments = taxremarks;
					vd6.DebitAmount = Conversion.ToDouble(SalesTax);
					vh.voucherDetailList.Add(vd6);
					VoucherDetail vd16 = new VoucherDetail();
					vd16.AccountId = obj.SaleTaxAccountId;
					vd16.AgainstAccountId = CustomerGlId;
					vd16.Comments = taxremarks;
					vd16.CreditAmount = Conversion.ToDouble(SalesTax);
					vh.voucherDetailList.Add(vd16);
				}
				List<CommisionAgentBillTaxDetail> taxList = obj.CommisionAgentBillTaxDetailList ?? new List<CommisionAgentBillTaxDetail>();
				if (taxList.Any())
				{
					List<CommisionAgentBillTaxDetail> purchaseTaxes = taxList.Where((CommisionAgentBillTaxDetail x) => x.EntrySideId == 1 && x.TaxAmount > 0m).ToList();
					if (purchaseTaxes.Any())
					{
						List<string> parts2 = new List<string>();
						parts2.AddRange((from x in purchaseTaxes
							where x.TaxAmount > 0m
							select $"Purchase Tax - {x.TaxTypeName} @ {x.TaxPercantage}% Amount: {x.TaxAmount:N2}").Distinct());
						if (vehicleNos.Any())
						{
							string label2 = ((vehicleNos.Count == 1) ? "Vehicle No" : "Vehicle Nos");
							parts2.Add("Against " + label2 + ": " + string.Join(", ", vehicleNos));
						}
						string purchaseRemarks = string.Join(" | ", parts2);
						foreach (CommisionAgentBillTaxDetail grp2 in purchaseTaxes)
						{
							if (grp2.TaxAccountId == 0)
							{
								throw new Exception("Purchase Tax Account is required.");
							}
							decimal amount2 = grp2.TaxAmount;
							VoucherDetail vd2 = new VoucherDetail();
							vd2.AccountId = vh.RefAccountId;
							vd2.AgainstAccountId = grp2.TaxAccountId;
							vd2.Comments = purchaseRemarks;
							vd2.DebitAmount = Conversion.ToDouble(amount2);
							vd2.TaxTypeId = grp2.TaxTypeId;
							vh.voucherDetailList.Add(vd2);
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = grp2.TaxAccountId;
							vd4.AgainstAccountId = vh.RefAccountId;
							vd4.Comments = purchaseRemarks;
							vd4.CreditAmount = Conversion.ToDouble(amount2);
							vd4.TaxTypeId = grp2.TaxTypeId;
							vh.voucherDetailList.Add(vd4);
						}
					}
					List<CommisionAgentBillTaxDetail> saleTaxes = taxList.Where((CommisionAgentBillTaxDetail x) => x.EntrySideId == 2 && x.TaxAmount > 0m).ToList();
					if (saleTaxes.Any())
					{
						List<string> parts = new List<string>();
						parts.AddRange((from x in saleTaxes
							where x.TaxAmount > 0m
							select $"Sales Tax - {x.TaxTypeName} @ {x.TaxPercantage}% Amount: {x.TaxAmount:N2}").Distinct());
						if (vehicleNos.Any())
						{
							string label = ((vehicleNos.Count == 1) ? "Vehicle No" : "Vehicle Nos");
							parts.Add("Against " + label + ": " + string.Join(", ", vehicleNos));
						}
						string saleRemarks = string.Join(" | ", parts);
						foreach (CommisionAgentBillTaxDetail grp in saleTaxes)
						{
							if (grp.TaxAccountId == 0)
							{
								throw new Exception("Sale Tax Account is required.");
							}
							decimal amount = grp.TaxAmount;
							VoucherDetail vd1 = new VoucherDetail();
							vd1.AccountId = CustomerGlId;
							vd1.AgainstAccountId = grp.TaxAccountId;
							vd1.Comments = saleRemarks;
							vd1.CreditAmount = Conversion.ToDouble(amount);
							vd1.TaxTypeId = grp.TaxTypeId;
							vh.voucherDetailList.Add(vd1);
							VoucherDetail vd3 = new VoucherDetail();
							vd3.AccountId = grp.TaxAccountId;
							vd3.AgainstAccountId = CustomerGlId;
							vd3.Comments = saleRemarks;
							vd3.DebitAmount = Conversion.ToDouble(amount);
							vd3.TaxTypeId = grp.TaxTypeId;
							vh.voucherDetailList.Add(vd3);
						}
					}
				}
				vh.VoucherAmount = TotalAmount;
				vh.BillAmount = TotalAmount;
				double TradingProfit = TradingTotalDebit - TradingTotalCredit;
				if (TradingProfit != 0.0)
				{
					VoucherDetail vd5 = new VoucherDetail();
					vd5.AccountId = Conversion.ToInt(obj.TradingGlAccountId);
					vd5.AgainstAccountId = Conversion.ToInt(obj.TradingGlAccountId);
					vd5.Comments = Conversion.ToString(TradingRemarks);
					if (TradingProfit > 0.0)
					{
						vd5.DebitAmount = Conversion.ToDouble(TradingProfit);
					}
					else
					{
						vd5.CreditAmount = Math.Abs(Conversion.ToDouble(TradingProfit));
					}
					vh.voucherDetailList.Add(vd5);
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	private static string BuildSupplierRemarks(Architecture.Model.Inventory.InvCommAgentTradeBill obj, InvCommAgentTradeBillDetail item)
	{
		List<string> list = new List<string>();
		list.Add("Item Name : '" + item.ItemName + "' " + $"Qty : {item.Qty:#,##0.###} " + $"Weight : {item.BillWeight:#,##0.###}");
		List<string> lines = list;
		lines.Add($"Rate : {item.RatePurchase:#,##0.###} " + $"Amount : {item.ItemAmount:#,##0.###}");
		if (!string.IsNullOrWhiteSpace(item.VehicleNo))
		{
			lines.Add("Vehicle : " + item.VehicleNo);
		}
		return string.Join(" ", lines);
	}

	private static string BuildCustomerRemarks(Architecture.Model.Inventory.InvCommAgentTradeBill obj, InvCommAgentTradeBillDetail item)
	{
		List<string> list = new List<string>();
		list.Add("Item Name : '" + item.ItemName + "' " + $"Qty : {item.Qty:#,##0.###} " + $"Weight : {item.SaleBillWeight:#,##0.###}");
		List<string> lines = list;
		lines.Add($"Rate : {item.RateSale:#,##0.###} " + $"Amount : {item.SaleAmount:#,##0.###}");
		if (!string.IsNullOrWhiteSpace(item.VehicleNo))
		{
			lines.Add("Vehicle : " + item.VehicleNo);
		}
		return string.Join(" ", lines);
	}

	private static string BuildFreightRemarks(Architecture.Model.Inventory.InvCommAgentTradeBill obj, CommisionAgentBillFreightDetail item)
	{
		List<string> parts = new List<string>();
		if (!string.IsNullOrWhiteSpace(item.Remarks))
		{
			parts.Add(item.Remarks.Trim());
		}
		if (item.AddLessAmount != 0m)
		{
			parts.Add($"Freight: {item.Amount:#,##0.###}");
			parts.Add($"Add/Less: {item.AddLessAmount:#,##0.###}");
			parts.Add($"Net Freight: {item.NetAmount:#,##0.###}");
		}
		else
		{
			parts.Add($"Freight: {item.NetAmount:#,##0.###}");
		}
		List<string> vehicleNos = (from x in obj.InvCommAgentTradeBillDetailslist?.Where((InvCommAgentTradeBillDetail x) => !string.IsNullOrWhiteSpace(x.VehicleNo))
			select x.VehicleNo.Trim()).Distinct().ToList() ?? new List<string>();
		if (vehicleNos.Any())
		{
			string label = ((vehicleNos.Count == 1) ? "Vehicle No" : "Vehicle Nos");
			parts.Add("Against " + label + ": " + string.Join(", ", vehicleNos));
		}
		return string.Join(" | ", parts);
	}

	private static string GenerateTaxRemarks(string side, decimal percent, decimal amount, string taxTypeText)
	{
		return $"{side} Tax ({taxTypeText}) @ {percent}% = {amount:N2}";
	}

	public static int Save(Architecture.Model.Inventory.InvCommAgentTradeBill obj)
	{
		try
		{
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
			}
			else
			{
				obj.ActionId = 2;
			}
			return new Architecture.DAL.Inventory.InvCommAgentTradeBill().SetData(obj, "USP_InvCommAgentTradeBill_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvCommAgentTradeBill GetByID(int Id)
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
			new List<Architecture.Model.Inventory.InvCommAgentTradeBill>();
			return new Architecture.DAL.Inventory.InvCommAgentTradeBill().GetDate("USP_InvCommAgentTradeBill_GetAllMethods", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
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
			sqlCommand.CommandText = "[dbo].[USP_InvCommAgentTradeBill_GetAllMethods]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@Activity", "DeleteById");
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static int GenerateCode(Architecture.Model.Inventory.InvCommAgentTradeBill obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvCommAgentTradeBill>("USP_InvCommAgentTradeBill_GetAllMethods", p);
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

	public static int GenerateBranchSrCode(Architecture.Model.Inventory.InvCommAgentTradeBill obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateBranchSrCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvCommAgentTradeBill>("USP_InvCommAgentTradeBill_GetAllMethods", p);
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
			if (obj.GlAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TradingAccountId",
					Value = obj.GlAccountId
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
			if (obj.CustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerId",
					Value = obj.CustomerId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvCommAgentTradeBill>("USP_InvCommAgentTradeBill_GetAllMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromCommOrder(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_DropDownFillFromInvCommAgentTradeBill]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommissionBillSupplierCustomerAndIncomeTransactionsSlip(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("USP_CommissionBillSupplierCustomerAndIncomeTransactions", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TradeBillSlipAndRegister(ReportsParameters obj)
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("Sp_InvCommAgentTradeBill_SlipandRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable TradeBillSlipAndRegisterSupReprt(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@PihId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("SP_CommisionAgentBillOtherExpense_SubRpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommissionTradeRegister(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
				});
			}
			if (obj.ParentCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ParentCategoryId",
					Value = obj.ParentCategoryId
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.VehicleNo != string.Empty && obj.VehicleNo != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
				});
			}
			if (obj.DeliveryTerm != string.Empty && obj.DeliveryTerm != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DeliveryTerm",
					Value = obj.DeliveryTerm
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
					ParameterName = "@BrokerId",
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
			if (obj.GlAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TradingGlAccountId",
					Value = obj.GlAccountId
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
			if (obj.CustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CustomerId",
					Value = obj.CustomerId
				});
			}
			if (obj.ReportType != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.ReportType
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_CommissionAgentTrade_Register", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DataForApproval(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@OrganizationId", obj.OrganizationId),
				new SqlParameter("@CompanyId", obj.CompanyId),
				new SqlParameter("@BranchesId", obj.BranchesId),
				new SqlParameter("@FinancialYearId", obj.FinancialYearId)
			};
			if (obj.DocumentTypeId != 0)
			{
				parameters.Add(new SqlParameter("@DocumentTypeId", obj.DocumentTypeId));
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				parameters.Add(new SqlParameter("@FromDate", obj.FromDate));
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				parameters.Add(new SqlParameter("@ToDate", obj.ToDate));
			}
			if (obj.FromDocNo != 0.0)
			{
				parameters.Add(new SqlParameter("@FromDocNo", obj.FromDocNo));
			}
			if (obj.ToDocNo != 0.0)
			{
				parameters.Add(new SqlParameter("@ToDocNo", obj.ToDocNo));
			}
			if (obj.SupplierId != 0)
			{
				parameters.Add(new SqlParameter("@SupplierId", obj.SupplierId));
			}
			if (obj.BuyerId != 0)
			{
				parameters.Add(new SqlParameter("@BuyerId", obj.BuyerId));
			}
			if (obj.AccountId != 0)
			{
				parameters.Add(new SqlParameter("@TradingAccountId", obj.AccountId));
			}
			if (obj.ParentCategoryId != 0)
			{
				parameters.Add(new SqlParameter("@ParentCategoryId", obj.ParentCategoryId));
			}
			if (!string.IsNullOrEmpty(obj.ParentCategoryIds))
			{
				parameters.Add(new SqlParameter("@ParentItemIds", obj.ParentCategoryIds));
			}
			if (obj.ItemId != 0)
			{
				parameters.Add(new SqlParameter("@ItemId", obj.ItemId));
			}
			if (obj.ApprovedStatus.HasValue)
			{
				parameters.Add(new SqlParameter("@IsApproved", obj.ApprovedStatus));
			}
			if (obj.ReasonId != 0)
			{
				parameters.Add(new SqlParameter
				{
					ParameterName = "@ReasonId",
					Value = obj.ReasonId
				});
			}
			if (obj.SpecialApprovalDocs != 0)
			{
				parameters.Add(new SqlParameter
				{
					ParameterName = "@CallForSpecialApproval",
					Value = 1
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderHeader>("USP_InvCommAgentTradeBill_DataForApproval", parameters);
		}
		catch
		{
			throw;
		}
	}

	public static void UpdateApprovalStatus(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		DataTable dt = new DataTable();
		dt.Columns.Add("OrganizationId", typeof(int));
		dt.Columns.Add("CompanyId", typeof(int));
		dt.Columns.Add("FinancialYearId", typeof(int));
		dt.Columns.Add("DocumentTypeId", typeof(int));
		dt.Columns.Add("ApprovedUserId", typeof(int));
		dt.Columns.Add("ReqType", typeof(string));
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("ApprovalRemarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.OrganizationId, item.CompanyId, item.FinancialYearId, item.DocumentTypeId, item.EntryUser, ((object)item.ReqType) ?? ((object)DBNull.Value), item.Id, ((object)item.Comments) ?? ((object)DBNull.Value));
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("[dbo].[USP_InvCommAgentTradeBill_Approval]", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ApprovalData", dt).SqlDbType = SqlDbType.Structured;
				cmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static void UpdateSpecialApprovalStatus(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		DataTable dt = new DataTable();
		dt.Columns.Add("OrganizationId", typeof(int));
		dt.Columns.Add("CompanyId", typeof(int));
		dt.Columns.Add("FinancialYearId", typeof(int));
		dt.Columns.Add("DocumentTypeId", typeof(int));
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("ReasonId", typeof(int));
		dt.Columns.Add("ReqType", typeof(string));
		dt.Columns.Add("RateCute", typeof(double));
		dt.Columns.Add("ApprovedUserId", typeof(int));
		dt.Columns.Add("ApprovalRemarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.OrganizationId, item.CompanyId, item.FinancialYearId, item.DocumentTypeId, item.Id, item.ReasonId, ((object)item.ReqType) ?? ((object)DBNull.Value), ((object)item.Rate) ?? DBNull.Value, item.EntryUser, ((object)item.Comments) ?? ((object)DBNull.Value));
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("[dbo].[USP_InvCommAgentTradeBill_UpdateSpecialApproval]", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ApprovalData", dt).SqlDbType = SqlDbType.Structured;
				cmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}
}
