using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Export;

public class ExImFCBankReceipts
{
	public static DataTable GetPartyInvoicesAgainstFIFO(Architecture.Model.Export.ExImFCBankReceipts obj)
	{
		try
		{
			DataTable dtResult = new DataTable();
			dtResult.Columns.Add("InvoiceFcyAmount", typeof(decimal));
			dtResult.Columns.Add("ConversionAmount", typeof(decimal));
			dtResult.Columns.Add("ExchangeRate", typeof(decimal));
			dtResult.Columns.Add("Remarks", typeof(string));
			string PartyRemarks = "";
			obj.FcyBankReceiptBreakUpofPartyInvoiceslist = new List<FcyBankReceiptBreakUpofPartyInvoices>();
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
				ParameterName = "@CustomerId",
				Value = obj.SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FcyId",
				Value = obj.MultiCurrencyId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FcyBankReceiptId",
					Value = obj.Id
				});
			}
			DataTable dtFIFO = GenericProvider.GetDataTableProc<FcyBankReceiptBreakUpofPartyInvoices>("USP_GetPendingPartyInvoices", p);
			if (dtFIFO != null && dtFIFO.Rows.Count > 0)
			{
				double num = Conversion.ToDouble(dtFIFO.Compute("SUM(BalanceAmount)", ""));
				double TotalRequriedAmount = obj.FcGrossAmount;
				if (num > 0.0)
				{
					double Required = TotalRequriedAmount;
					double Available = 0.0;
					double Consumed = 0.0;
					decimal ConversionAmount = default(decimal);
					decimal InvoiceFcyAmount = default(decimal);
					for (int i = 0; i < dtFIFO.Rows.Count; i++)
					{
						FcyBankReceiptBreakUpofPartyInvoices objInvoice = new FcyBankReceiptBreakUpofPartyInvoices();
						Available = Conversion.ToDouble(dtFIFO.Rows[i]["BalanceAmount"]);
						if (Available > 0.0 && Available <= Required - Consumed)
						{
							Consumed += Available;
							objInvoice.RefDocId = Conversion.ToInt(dtFIFO.Rows[i]["InvoiceId"]);
							objInvoice.RealizedAmount = Conversion.ToDecimal(Available);
							obj.FcyBankReceiptBreakUpofPartyInvoiceslist.Add(objInvoice);
							PartyRemarks = " FcyCode " + obj.CurrencyCode + " @Rate " + Conversion.ToString(dtFIFO.Rows[i]["ExchangeRate"]) + " Invoice# " + Conversion.ToString(dtFIFO.Rows[i]["InvoiceNo"]) + " InvoiceAmount " + objInvoice.RealizedAmount;
							ConversionAmount = objInvoice.RealizedAmount * Conversion.ToDecimal(dtFIFO.Rows[i]["ExchangeRate"]);
							dtResult.Rows.Add(objInvoice.RealizedAmount, ConversionAmount, Conversion.ToDecimal(dtFIFO.Rows[i]["ExchangeRate"]), PartyRemarks);
						}
						else if (Available >= Required - Consumed)
						{
							objInvoice.RefDocId = Conversion.ToInt(dtFIFO.Rows[i]["InvoiceId"]);
							objInvoice.RealizedAmount = Conversion.ToDecimal(Required - Consumed);
							obj.FcyBankReceiptBreakUpofPartyInvoiceslist.Add(objInvoice);
							InvoiceFcyAmount += objInvoice.RealizedAmount;
							Consumed += Conversion.ToDouble(objInvoice.RealizedAmount);
							ConversionAmount = objInvoice.RealizedAmount * Conversion.ToDecimal(dtFIFO.Rows[i]["ExchangeRate"]);
							PartyRemarks = " FcyCode " + obj.CurrencyCode + " @Rate " + Conversion.ToString(dtFIFO.Rows[i]["ExchangeRate"]) + " Invoice# " + Conversion.ToString(dtFIFO.Rows[i]["InvoiceNo"]) + " InvoiceAmount " + objInvoice.RealizedAmount;
							dtResult.Rows.Add(objInvoice.RealizedAmount, ConversionAmount, Conversion.ToDecimal(dtFIFO.Rows[i]["ExchangeRate"]), PartyRemarks);
						}
						if (TotalRequriedAmount == Consumed)
						{
							break;
						}
					}
				}
			}
			return dtResult;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static VoucherHead MakeVoucher(Architecture.Model.Export.ExImFCBankReceipts obj, VoucherInvoicesAdjustment objAdjusment = null, decimal TotalInvoiceAmount = 0m)
	{
		List<Architecture.Model.Export.ExImProceedsChargesType> dtCharges = new List<Architecture.Model.Export.ExImProceedsChargesType>();
		List<Architecture.Model.Inventory.SupplierCustomer> dtSupplier = new List<Architecture.Model.Inventory.SupplierCustomer>();
		VoucherHead vh = new VoucherHead();
		obj.gainLossBreakupslist = new List<GainLossBreakup>();
		try
		{
			List<SqlParameter> z = new List<SqlParameter>();
			z.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			z.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			z.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			dtCharges = new Architecture.DAL.Export.ExImProceedsChargesType().GetAll("Sp_ExImProceedsChargesType_GetAllMethod", z);
			double VoucherAmt = 0.0;
			double BillAmt = 0.0;
			string PartyName = "";
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocumentNo);
			vh.VoucherDate = obj.DocumentDate;
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
				Value = "ReadByOrganizationCompanyIdForExport"
			});
			dtSupplier = new Architecture.DAL.Inventory.SupplierCustomer().GetData("Sp_SupplierCustomer_GetAllMethod", p);
			if (dtSupplier.Count > 0)
			{
				List<Architecture.Model.Inventory.SupplierCustomer> find3 = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomer x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find3.Count <= 0)
				{
					throw new Exception("Supplier GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find3[0].GlAccountId);
				PartyName = Conversion.ToString(find3[0].CompanyName);
			}
			double ForignCharges = 0.0;
			if (obj.FcBankReceiptDetail.Count > 0)
			{
				foreach (ExImFcBankReceiptsDeductions r5 in obj.FcBankReceiptDetail.ToList())
				{
					if (r5.FcAmount > 0.0 && obj.ExchangeRate > 0.0)
					{
						ForignCharges += r5.FcAmount;
					}
				}
			}
			string Remarks = "";
			if (obj.ExmLcPaymentTerm == "Invoice Payment")
			{
				if (obj.Remarks != null && obj.Remarks != string.Empty)
				{
					Remarks = Remarks + obj.Remarks + "  ";
				}
				Remarks = Remarks + "INVOICE NO " + obj.InvoiceNo + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount;
				if (ForignCharges > 0.0)
				{
					double NetFcy = obj.FcGrossAmount - ForignCharges;
					Remarks = Remarks + " - " + ForignCharges + " = " + NetFcy;
				}
				Remarks = Remarks + " @" + obj.ExchangeRate + " REALIZED IN " + obj.CompanyName + "  AS DATED " + obj.DocumentDate.ToString("dd-MMM-yyyy");
			}
			vh.Remarks = Remarks;
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchId;
			vh.ProjectId = obj.ProjectId;
			vh.ManualBillNo = Conversion.ToString(obj.ReferenceNo);
			vh.DueDate = DateTime.Now;
			vh.DueDays = 0;
			vh.ExchangeCurrencyRate = obj.ExchangeRate;
			vh.FcAmount = obj.FcGrossAmount;
			vh.MultiCurrencyId = obj.MultiCurrencyId;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.PostUser = 0;
			vh.voucherDetailList = new List<VoucherDetail>();
			if (obj.DocumentTypeId == 203)
			{
				bool GainLossConfig = false;
				GainLossConfig = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FCYReceiptAutoGainAndLoss"));
				if (!GainLossConfig)
				{
					decimal FTTAmount = default(decimal);
					if (obj.FcyBankReceiptBreakUpList.Count > 0)
					{
						foreach (Architecture.Model.Export.FcyBankReceiptBreakUp item2 in obj.FcyBankReceiptBreakUpList)
						{
							FTTAmount += item2.AgencyComm;
						}
					}
					if (obj.DebitAccountTypeId == 2 && obj.ExchangeRate > 0.0 && obj.ThirdCurrencyReceiverExchangeRate > 0.0 && obj.ThirdCurrencyReceiverFcyAmount > 0.0)
					{
						double ReceiverLcyAmount = 0.0;
						double ThirdFcyGainLoss = 0.0;
						ThirdFcyGainLoss = obj.FcGrossAmount - obj.ThirdCurrencyReceiverFcyAmount;
						ReceiverLcyAmount = Conversion.ToDouble(obj.ThirdCurrencyReceiverFcyAmount) * obj.ExchangeRate;
						VoucherDetail vd11 = new VoucherDetail();
						vd11.AccountId = Conversion.ToInt(obj.BankGlDrAc);
						vd11.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd11.DebitAmount = ReceiverLcyAmount;
						VoucherAmt += vd11.DebitAmount;
						vd11.CreditAmount = Conversion.ToDouble(0);
						vd11.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Received From " + PartyName + " @ " + obj.ThirdCurrencyFcyExchangeRate;
						vd11.DMultiCurrencyId = obj.MultiCurrencyId;
						vd11.DExchangeCurrencyRate = obj.ExchangeRate;
						vd11.DCurrencyAmount = obj.ThirdCurrencyReceiverFcyAmount;
						vd11.ThirdCurrencyId = obj.DebitAccountCurrencyId;
						vd11.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
						vd11.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd11.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
						vd11.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
						vd11.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
						vh.voucherDetailList.Add(vd11);
						if (ThirdFcyGainLoss != 0.0)
						{
							VoucherDetail vd21 = new VoucherDetail();
							vd21.AccountId = obj.VoucherHeadId;
							vd21.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							if (ThirdFcyGainLoss > 0.0)
							{
								vd21.DebitAmount = Conversion.ToDouble(ThirdFcyGainLoss) * obj.ExchangeRate;
							}
							else
							{
								vd21.CreditAmount = Math.Abs(Conversion.ToDouble(ThirdFcyGainLoss)) * obj.ExchangeRate;
							}
							vd21.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate + "  Party Ex.Rate " + obj.ThirdCurrencyFcyExchangeRate;
							vd21.DMultiCurrencyId = obj.MultiCurrencyId;
							vd21.DExchangeCurrencyRate = obj.ExchangeRate;
							vd21.DCurrencyAmount = ThirdFcyGainLoss;
							vh.voucherDetailList.Add(vd21);
						}
					}
					else
					{
						VoucherDetail vd12 = new VoucherDetail();
						vd12.AccountId = Conversion.ToInt(obj.BankGlDrAc);
						vd12.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd12.DebitAmount = Conversion.ToDouble(obj.NetAmountRs);
						VoucherAmt += vd12.DebitAmount;
						vd12.CreditAmount = Conversion.ToDouble(0);
						vd12.Comments = obj.Remarks + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount;
						if (FTTAmount > 0m)
						{
							vd12.Comments = vd12.Comments + "  FTT " + FTTAmount;
						}
						vd12.Comments = vd12.Comments + " @ " + obj.ExchangeRate;
						vd12.DMultiCurrencyId = obj.MultiCurrencyId;
						vd12.DExchangeCurrencyRate = obj.ExchangeRate;
						double FcAmt = obj.NetAmountRs / obj.ExchangeRate;
						vd12.DCurrencyAmount = FcAmt;
						vd12.ThirdCurrencyId = obj.DebitAccountCurrencyId;
						vd12.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
						vd12.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd12.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
						vh.voucherDetailList.Add(vd12);
					}
					VoucherDetail vd7 = new VoucherDetail();
					vd7.AccountId = vh.RefAccountId;
					vd7.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
					vd7.Comments = obj.Remarks + "   " + obj.CurrencyCode + "  " + obj.FcGrossAmount + "  @ " + obj.ExchangeRate;
					vd7.DebitAmount = Conversion.ToDouble(0);
					vd7.CreditAmount = Conversion.ToDouble(obj.NetAmountRs);
					BillAmt += vd7.CreditAmount;
					vd7.DMultiCurrencyId = obj.MultiCurrencyId;
					vd7.DExchangeCurrencyRate = obj.ExchangeRate;
					double FcYAmt = obj.NetAmountRs / obj.ExchangeRate;
					vd7.DCurrencyAmount = FcYAmt;
					vd7.ThirdCurrencyId = obj.ThirdCurrencyId;
					vd7.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
					vd7.ThirdCurrencyAmount = ((obj.ThirdCurrencyAmount > 0.0 && obj.ThirdCurrencyFcyExchangeRate > 0.0) ? (vd7.DCurrencyAmount * obj.ThirdCurrencyFcyExchangeRate) : obj.ThirdCurrencyAmount);
					vd7.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
					vd7.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
					vh.voucherDetailList.Add(vd7);
					if (obj.FcFBCharges > 0.0)
					{
						if (obj.FBCDebitAccountId == 0)
						{
							throw new Exception("FBCDebitAccountId not found");
						}
						VoucherDetail vd32 = new VoucherDetail();
						vd32.AccountId = obj.FBCDebitAccountId;
						vd32.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd32.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
						vd32.DebitAmount = Conversion.ToDouble(obj.FcFBCharges) * ((obj.VoucherExchangeRate == 0.0) ? obj.ExchangeRate : obj.VoucherExchangeRate);
						vd32.DMultiCurrencyId = obj.MultiCurrencyId;
						vd32.DExchangeCurrencyRate = ((obj.VoucherExchangeRate == 0.0) ? obj.ExchangeRate : obj.VoucherExchangeRate);
						vd32.DCurrencyAmount = obj.FcFBCharges;
						vh.voucherDetailList.Add(vd32);
						VoucherDetail vd28 = new VoucherDetail();
						vd28.AccountId = vh.RefAccountId;
						vd28.AgainstAccountId = Conversion.ToInt(obj.FBCDebitAccountId);
						vd28.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
						vd28.CreditAmount = Conversion.ToDouble(obj.FcFBCharges) * ((obj.VoucherExchangeRate == 0.0) ? obj.ExchangeRate : obj.VoucherExchangeRate);
						BillAmt += vd28.CreditAmount;
						vd28.DMultiCurrencyId = obj.MultiCurrencyId;
						vd28.DExchangeCurrencyRate = ((obj.VoucherExchangeRate == 0.0) ? obj.ExchangeRate : obj.VoucherExchangeRate);
						vd28.DCurrencyAmount = obj.FcFBCharges;
						vd28.ThirdCurrencyId = obj.ThirdCurrencyId;
						vd28.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
						vd28.ThirdCurrencyAmount = ((obj.ThirdCurrencyAmount > 0.0 && obj.ThirdCurrencyFcyExchangeRate > 0.0) ? (vd28.CreditAmount / obj.ThirdCurrencyFcyExchangeRate) : obj.ThirdCurrencyAmount);
						vh.voucherDetailList.Add(vd28);
					}
					if (obj.FcBankReceiptDetail.Count > 0)
					{
						foreach (ExImFcBankReceiptsDeductions r4 in obj.FcBankReceiptDetail.ToList())
						{
							if (dtCharges.Count > 0)
							{
								List<Architecture.Model.Export.ExImProceedsChargesType> find = dtCharges.Where((Architecture.Model.Export.ExImProceedsChargesType x) => x.Id == Conversion.ToInt(r4.ExImProceedsChargesTypeId)).ToList();
								if (find.Count <= 0)
								{
									throw new Exception("Charges AccountType GLAccountId not Found");
								}
								VoucherDetail vd = new VoucherDetail();
								vd.AccountId = Conversion.ToInt(find[0].ProceedsChargesGlAccount);
								vd.AgainstAccountId = obj.BankGlDrAc;
								if (r4.ProceedsRemarks == string.Empty)
								{
									vd.Comments = r4.ProceedsRemarks + "  " + find[0].ProceedsChargesTypedescription;
								}
								else
								{
									vd.Comments = r4.ProceedsRemarks;
								}
								vd.DebitAmount = Conversion.ToDouble(r4.RsAmount);
								VoucherAmt += vd.DebitAmount;
								if (r4.Type == "Local")
								{
									double FYAmount = r4.RsAmount / obj.ExchangeRate;
									vd.DCurrencyAmount = FYAmount;
									vd.DExchangeCurrencyRate = obj.ExchangeRate;
									vd.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								else
								{
									vd.DCurrencyAmount = r4.FcAmount;
									vd.DExchangeCurrencyRate = obj.ExchangeRate;
									vd.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								vd.CreditAmount = Conversion.ToDouble(0);
								vh.voucherDetailList.Add(vd);
								VoucherDetail vd4 = new VoucherDetail();
								vd4.AccountId = obj.BankGlDrAc;
								vd4.AgainstAccountId = Conversion.ToInt(find[0].ProceedsChargesGlAccount);
								if (r4.ProceedsRemarks == string.Empty)
								{
									vd4.Comments = r4.ProceedsRemarks + "  " + find[0].ProceedsChargesTypedescription;
								}
								else
								{
									vd4.Comments = r4.ProceedsRemarks;
								}
								vd4.DebitAmount = Conversion.ToDouble(0);
								vd4.CreditAmount = Conversion.ToDouble(r4.RsAmount);
								BillAmt += vd4.CreditAmount;
								if (r4.Type == "Local")
								{
									double FcYAmount = r4.RsAmount / obj.ExchangeRate;
									vd4.DCurrencyAmount = FcYAmount;
									vd4.DExchangeCurrencyRate = obj.ExchangeRate;
									vd4.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								else
								{
									vd4.DCurrencyAmount = r4.FcAmount;
									vd4.DExchangeCurrencyRate = obj.ExchangeRate;
									vd4.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								vd4.ThirdCurrencyId = obj.DebitAccountCurrencyId;
								vd4.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
								vd4.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd4.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
								vh.voucherDetailList.Add(vd4);
							}
						}
						vh.VoucherAmount = VoucherAmt;
						vh.BillAmount = BillAmt;
					}
				}
				else if (GainLossConfig)
				{
					double PartyAmount = 0.0;
					double GainlossAmt = 0.0;
					string InvoiceRemarks = "";
					double DeductionAmount = 0.0;
					foreach (ExImFcBankReceiptsDeductions r6 in obj.FcBankReceiptDetail.ToList())
					{
						DeductionAmount += r6.RsAmount;
					}
					string Partyremarks = "";
					if ((obj.ExmLcPaymentTerm == "Invoice Payment" && obj.VoucherExchangeRate > 0.0) || TotalInvoiceAmount > 0m)
					{
						if (obj.DebitAccountTypeId == 2 && obj.ExchangeRate > 0.0 && obj.ThirdCurrencyReceiverExchangeRate > 0.0 && obj.ThirdCurrencyReceiverFcyAmount > 0.0)
						{
							double ReceiverLcyAmount2 = 0.0;
							double ThirdFcyGainLoss2 = 0.0;
							ThirdFcyGainLoss2 = obj.FcGrossAmount - obj.ThirdCurrencyReceiverFcyAmount;
							ReceiverLcyAmount2 = Conversion.ToDouble(obj.ThirdCurrencyReceiverFcyAmount) * obj.ExchangeRate;
							VoucherDetail vd13 = new VoucherDetail();
							vd13.AccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd13.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd13.DebitAmount = ReceiverLcyAmount2;
							VoucherAmt += vd13.DebitAmount;
							vd13.CreditAmount = Conversion.ToDouble(0);
							vd13.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Received From " + PartyName + " @ " + obj.ThirdCurrencyFcyExchangeRate + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount + " @ " + obj.ExchangeRate + " Receiver Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate;
							vd13.DMultiCurrencyId = obj.MultiCurrencyId;
							vd13.DExchangeCurrencyRate = obj.ExchangeRate;
							vd13.DCurrencyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vd13.ThirdCurrencyId = obj.DebitAccountCurrencyId;
							vd13.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
							vd13.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd13.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
							vd13.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
							vd13.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vh.voucherDetailList.Add(vd13);
							if (ThirdFcyGainLoss2 != 0.0)
							{
								VoucherDetail vd24 = new VoucherDetail();
								vd24.AccountId = obj.VoucherHeadId;
								vd24.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								if (ThirdFcyGainLoss2 > 0.0)
								{
									vd24.DebitAmount = Conversion.ToDouble(ThirdFcyGainLoss2) * obj.ExchangeRate;
								}
								else
								{
									vd24.CreditAmount = Math.Abs(Conversion.ToDouble(ThirdFcyGainLoss2)) * obj.ExchangeRate;
								}
								vd24.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate + "  Party Ex.Rate " + obj.ThirdCurrencyFcyExchangeRate;
								vd24.DMultiCurrencyId = obj.MultiCurrencyId;
								vd24.DExchangeCurrencyRate = obj.ExchangeRate;
								vd24.DCurrencyAmount = ThirdFcyGainLoss2;
								vh.voucherDetailList.Add(vd24);
							}
						}
						else
						{
							VoucherDetail vd14 = new VoucherDetail();
							vd14.AccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd14.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd14.DebitAmount = Conversion.ToDouble(obj.NetAmountRs);
							VoucherAmt += vd14.DebitAmount;
							vd14.CreditAmount = Conversion.ToDouble(0);
							vd14.Comments = Remarks;
							vd14.DMultiCurrencyId = obj.MultiCurrencyId;
							vd14.DExchangeCurrencyRate = obj.ExchangeRate;
							double FcAmt2 = obj.NetAmountRs / obj.ExchangeRate;
							vd14.DCurrencyAmount = FcAmt2;
							vd14.ThirdCurrencyId = obj.DebitAccountCurrencyId;
							vd14.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
							vd14.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd14.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
							vh.voucherDetailList.Add(vd14);
						}
						if (TotalInvoiceAmount > 0m)
						{
							PartyAmount = Conversion.ToDouble(TotalInvoiceAmount);
							obj.VoucherExchangeRate = PartyAmount / obj.FcGrossAmount;
							InvoiceRemarks = InvoiceRemarks + "Invoice Amounts: " + string.Join(", ", objAdjusment.VoucherInvoicesAdjustmentList.Select((VoucherInvoicesAdjustment x) => $"{x.AdjustmentAmount} (Rate: {x.ExchangeRate})"));
						}
						else
						{
							PartyAmount = obj.FcGrossAmount * obj.VoucherExchangeRate;
						}
						GainlossAmt = obj.BankDrAmount + DeductionAmount - PartyAmount;
						Remarks = Remarks + " INVOICE EXCHANGE RATE IS @ " + obj.VoucherExchangeRate;
						VoucherDetail vd20 = new VoucherDetail();
						vd20.AccountId = vh.RefAccountId;
						vd20.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
						vd20.DebitAmount = Conversion.ToDouble(0);
						vd20.CreditAmount = Conversion.ToDouble(PartyAmount);
						vd20.DMultiCurrencyId = obj.MultiCurrencyId;
						vd20.DExchangeCurrencyRate = obj.ExchangeRate;
						double FcYAmtG = PartyAmount / obj.VoucherExchangeRate;
						vd20.DCurrencyAmount = FcYAmtG;
						vd20.Comments = ((!string.IsNullOrEmpty(InvoiceRemarks)) ? (Remarks + "  " + InvoiceRemarks) : Remarks);
						Partyremarks += vd20.Comments;
						BillAmt += vd20.CreditAmount;
						vd20.ThirdCurrencyId = obj.ThirdCurrencyId;
						vd20.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
						vd20.ThirdCurrencyAmount = ((obj.ThirdCurrencyAmount > 0.0 && obj.ThirdCurrencyFcyExchangeRate > 0.0) ? (vd20.DCurrencyAmount * obj.ThirdCurrencyFcyExchangeRate) : obj.ThirdCurrencyAmount);
						vh.voucherDetailList.Add(vd20);
						if (obj.FcFBCharges > 0.0)
						{
							if (obj.FBCDebitAccountId == 0)
							{
								throw new Exception("FBCDebitAccountId not found");
							}
							VoucherDetail vd33 = new VoucherDetail();
							vd33.AccountId = obj.FBCDebitAccountId;
							vd33.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd33.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
							vd33.DebitAmount = Conversion.ToDouble(obj.FcFBCharges) * obj.VoucherExchangeRate;
							vd33.DMultiCurrencyId = obj.MultiCurrencyId;
							vd33.DExchangeCurrencyRate = obj.VoucherExchangeRate;
							vd33.DCurrencyAmount = obj.FcFBCharges;
							vh.voucherDetailList.Add(vd33);
							VoucherDetail vd29 = new VoucherDetail();
							vd29.AccountId = vh.RefAccountId;
							vd29.AgainstAccountId = Conversion.ToInt(obj.FBCDebitAccountId);
							vd29.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
							vd29.CreditAmount = Conversion.ToDouble(obj.FcFBCharges) * obj.VoucherExchangeRate;
							BillAmt += vd29.CreditAmount;
							vd29.DMultiCurrencyId = obj.MultiCurrencyId;
							vd29.DExchangeCurrencyRate = obj.VoucherExchangeRate;
							vd29.DCurrencyAmount = obj.FcFBCharges;
							vd29.ThirdCurrencyId = obj.ThirdCurrencyId;
							vd29.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
							vd29.ThirdCurrencyAmount = ((obj.ThirdCurrencyAmount > 0.0 && obj.ThirdCurrencyFcyExchangeRate > 0.0) ? (vd29.DCurrencyAmount * obj.ThirdCurrencyFcyExchangeRate) : obj.ThirdCurrencyAmount);
							vh.voucherDetailList.Add(vd29);
						}
						string GainLossRemarks = " FcyCode " + obj.CurrencyCode + " @Rate " + Conversion.ToString(obj.VoucherExchangeRate) + " InvoiceAmount " + obj.FcGrossAmount;
						GainLossBreakup gl = new GainLossBreakup();
						gl.FcyAmount = Conversion.ToDouble(obj.FcGrossAmount);
						gl.LocalAmount = Conversion.ToDouble(PartyAmount);
						gl.ExchangeRate = Conversion.ToDouble(obj.VoucherExchangeRate);
						gl.Type = "Invoice";
						gl.Remarks = ((!string.IsNullOrEmpty(InvoiceRemarks)) ? (GainLossRemarks + "  " + InvoiceRemarks) : GainLossRemarks);
						obj.gainLossBreakupslist.Add(gl);
						if (GainlossAmt != 0.0)
						{
							if (TotalInvoiceAmount > 0m)
							{
								foreach (VoucherInvoicesAdjustment item3 in objAdjusment.VoucherInvoicesAdjustmentList.ToList())
								{
									decimal InvoieWiseGainLoss = default(decimal);
									Architecture.Model.Export.ExImInvoice objInvoie = ExImInvoice.GetInvoiecHeaderByID(item3.ExImInvoiceId);
									obj.VoucherHeadId = ((objInvoie.CreditAccountId > 0) ? objInvoie.CreditAccountId : obj.VoucherHeadId);
									if (obj.VoucherHeadId == 0)
									{
										throw new Exception("Gain & Loss Account Not found....");
									}
									InvoieWiseGainLoss = (item3.ExchangeRate - Conversion.ToDecimal(obj.ExchangeRate)) * item3.AdjustmentAmount;
									if (InvoieWiseGainLoss != 0m)
									{
										VoucherDetail vd22 = new VoucherDetail();
										vd22.AccountId = obj.VoucherHeadId;
										vd22.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
										if (InvoieWiseGainLoss > 0m)
										{
											vd22.DebitAmount = Conversion.ToDouble(InvoieWiseGainLoss);
										}
										else
										{
											vd22.CreditAmount = Math.Abs(Conversion.ToDouble(InvoieWiseGainLoss));
										}
										vd22.Comments = InvoiceRemarks;
										vd22.DMultiCurrencyId = obj.MultiCurrencyId;
										vd22.DExchangeCurrencyRate = obj.ExchangeRate;
										double FcYAmtGain = Conversion.ToDouble(InvoieWiseGainLoss) / obj.VoucherExchangeRate;
										vd22.DCurrencyAmount = FcYAmtGain;
										vd22.RateCutAmount = Conversion.ToDouble(InvoieWiseGainLoss);
										vh.voucherDetailList.Add(vd22);
									}
								}
							}
							else
							{
								if (obj.VoucherHeadId == 0)
								{
									throw new Exception("Gain & Loss Account Not found....");
								}
								VoucherDetail vd23 = new VoucherDetail();
								vd23.AccountId = obj.VoucherHeadId;
								vd23.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								if (GainlossAmt > 0.0)
								{
									vd23.DebitAmount = Conversion.ToDouble(0);
									vd23.CreditAmount = Conversion.ToDouble(GainlossAmt);
								}
								else
								{
									vd23.CreditAmount = Conversion.ToDouble(0);
									vd23.DebitAmount = Math.Abs(Conversion.ToDouble(GainlossAmt));
								}
								vd23.Comments = Remarks;
								vd23.DMultiCurrencyId = obj.MultiCurrencyId;
								vd23.DExchangeCurrencyRate = obj.ExchangeRate;
								double FcYAmtGain2 = Math.Abs(GainlossAmt) / obj.VoucherExchangeRate;
								vd23.DCurrencyAmount = FcYAmtGain2;
								vd23.RateCutAmount = GainlossAmt;
								vh.voucherDetailList.Add(vd23);
							}
						}
					}
					else if (obj.ExmLcPaymentTerm == "Multi Invoices Payment")
					{
						if ((obj.TransTypeId == 2 || obj.TransTypeId == 3) && obj.DebitAccountTypeId == 1)
						{
							if (obj.FcyBankReceiptBreakUpList.Count == 0)
							{
								throw new Exception("FcyBankReceiptBreakUpList not found");
							}
							foreach (Architecture.Model.Export.FcyBankReceiptBreakUp item in obj.FcyBankReceiptBreakUpList)
							{
								VoucherDetail vd19 = new VoucherDetail();
								vd19.AccountId = Conversion.ToInt(obj.BankGlDrAc);
								vd19.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd19.DebitAmount = Conversion.ToDouble(item.RealizedAmount) * obj.ExchangeRate;
								VoucherAmt += vd19.DebitAmount;
								vd19.CreditAmount = Conversion.ToDouble(0);
								vd19.Comments = obj.Remarks + "  " + obj.CurrencyCode + "  " + item.RealizedAmount;
								if (item.AgencyComm > 0m)
								{
									vd19.Comments = vd19.Comments + "  FTT " + item.AgencyComm;
								}
								vd19.Comments = vd19.Comments + " @ " + obj.ExchangeRate;
								vd19.DMultiCurrencyId = obj.MultiCurrencyId;
								vd19.DExchangeCurrencyRate = obj.ExchangeRate;
								vd19.DCurrencyAmount = Conversion.ToDouble(item.RealizedAmount);
								vh.voucherDetailList.Add(vd19);
								if (obj.FcBankReceiptDetail.Count <= 0)
								{
									continue;
								}
								List<ExImFcBankReceiptsDeductions> FDBCNOLIST = obj.FcBankReceiptDetail.Where((ExImFcBankReceiptsDeductions x) => Conversion.ToString(x.FDBCNo).ToUpper() == Conversion.ToString(item.ReceiverRefNo).ToUpper()).ToList();
								if (FDBCNOLIST.Count > 0)
								{
									foreach (ExImFcBankReceiptsDeductions r3 in FDBCNOLIST.ToList())
									{
										if (dtCharges.Count > 0)
										{
											List<Architecture.Model.Export.ExImProceedsChargesType> find4 = dtCharges.Where((Architecture.Model.Export.ExImProceedsChargesType x) => x.Id == Conversion.ToInt(r3.ExImProceedsChargesTypeId)).ToList();
											if (find4.Count > 0)
											{
												VoucherDetail vd6 = new VoucherDetail();
												vd6.AccountId = obj.BankGlDrAc;
												vd6.AgainstAccountId = Conversion.ToInt(find4[0].ProceedsChargesGlAccount);
												if (r3.ProceedsRemarks == string.Empty)
												{
													vd6.Comments = r3.ProceedsRemarks + "  " + find4[0].ProceedsChargesTypedescription;
												}
												else
												{
													vd6.Comments = r3.ProceedsRemarks;
												}
												vd6.DebitAmount = Conversion.ToDouble(0);
												vd6.CreditAmount = Conversion.ToDouble(r3.RsAmount);
												BillAmt += vd6.CreditAmount;
												if (r3.Type == "Local")
												{
													double FcYAmount3 = r3.RsAmount / obj.ExchangeRate;
													vd6.DCurrencyAmount = FcYAmount3;
													vd6.DExchangeCurrencyRate = obj.ExchangeRate;
													vd6.DMultiCurrencyId = obj.MultiCurrencyId;
												}
												else
												{
													vd6.DCurrencyAmount = r3.FcAmount;
													vd6.DExchangeCurrencyRate = obj.ExchangeRate;
													vd6.DMultiCurrencyId = obj.MultiCurrencyId;
												}
												vh.voucherDetailList.Add(vd6);
												continue;
											}
											throw new Exception("Charges AccountType GLAccountId not Found");
										}
										throw new Exception("Charges data not Found");
									}
									foreach (ExImFcBankReceiptsDeductions r2 in FDBCNOLIST.ToList())
									{
										if (dtCharges.Count > 0)
										{
											List<Architecture.Model.Export.ExImProceedsChargesType> find5 = dtCharges.Where((Architecture.Model.Export.ExImProceedsChargesType x) => x.Id == Conversion.ToInt(r2.ExImProceedsChargesTypeId)).ToList();
											if (find5.Count > 0)
											{
												VoucherDetail vd3 = new VoucherDetail();
												vd3.AccountId = Conversion.ToInt(find5[0].ProceedsChargesGlAccount);
												vd3.AgainstAccountId = obj.BankGlDrAc;
												if (r2.ProceedsRemarks == string.Empty)
												{
													vd3.Comments = r2.ProceedsRemarks + "  " + find5[0].ProceedsChargesTypedescription;
												}
												else
												{
													vd3.Comments = r2.ProceedsRemarks;
												}
												vd3.DebitAmount = Conversion.ToDouble(r2.RsAmount);
												VoucherAmt += vd3.DebitAmount;
												if (r2.Type == "Local")
												{
													double FYAmount3 = r2.RsAmount / obj.ExchangeRate;
													vd3.DCurrencyAmount = FYAmount3;
													vd3.DExchangeCurrencyRate = obj.ExchangeRate;
													vd3.DMultiCurrencyId = obj.MultiCurrencyId;
												}
												else
												{
													vd3.DCurrencyAmount = r2.FcAmount;
													vd3.DExchangeCurrencyRate = obj.ExchangeRate;
													vd3.DMultiCurrencyId = obj.MultiCurrencyId;
												}
												vd3.CreditAmount = Conversion.ToDouble(0);
												vh.voucherDetailList.Add(vd3);
												continue;
											}
											throw new Exception("Charges AccountType GLAccountId not Found");
										}
										throw new Exception("Charges data not Found");
									}
									vh.VoucherAmount = VoucherAmt;
									vh.BillAmount = BillAmt;
									continue;
								}
								throw new Exception("FDBCNO NOT MATCH INVOICE WISE BREAKUP AND CHARGES GRID");
							}
						}
						else if (obj.DebitAccountTypeId == 2 && obj.ExchangeRate > 0.0 && obj.ThirdCurrencyReceiverExchangeRate > 0.0 && obj.ThirdCurrencyReceiverFcyAmount > 0.0)
						{
							double ReceiverLcyAmount4 = 0.0;
							double ThirdFcyGainLoss4 = 0.0;
							ThirdFcyGainLoss4 = obj.FcGrossAmount - obj.ThirdCurrencyReceiverFcyAmount;
							ReceiverLcyAmount4 = Conversion.ToDouble(obj.ThirdCurrencyReceiverFcyAmount) * obj.ExchangeRate;
							VoucherDetail vd18 = new VoucherDetail();
							vd18.AccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd18.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd18.DebitAmount = ReceiverLcyAmount4;
							VoucherAmt += vd18.DebitAmount;
							vd18.CreditAmount = Conversion.ToDouble(0);
							vd18.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Received From " + PartyName + " @ " + obj.ThirdCurrencyFcyExchangeRate + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount + " @ " + obj.ExchangeRate + " Receiver Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate;
							vd18.DMultiCurrencyId = obj.MultiCurrencyId;
							vd18.DExchangeCurrencyRate = obj.ExchangeRate;
							vd18.DCurrencyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vd18.ThirdCurrencyId = obj.ThirdCurrencyId;
							vd18.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
							vd18.ThirdCurrencyAmount = obj.ThirdCurrencyAmount;
							vd18.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
							vd18.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vh.voucherDetailList.Add(vd18);
							if (ThirdFcyGainLoss4 != 0.0)
							{
								VoucherDetail vd27 = new VoucherDetail();
								vd27.AccountId = obj.VoucherHeadId;
								vd27.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								if (ThirdFcyGainLoss4 > 0.0)
								{
									vd27.DebitAmount = Conversion.ToDouble(ThirdFcyGainLoss4) * obj.ExchangeRate;
								}
								else
								{
									vd27.CreditAmount = Math.Abs(Conversion.ToDouble(ThirdFcyGainLoss4)) * obj.ExchangeRate;
								}
								vd27.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate + "  Party Ex.Rate " + obj.ThirdCurrencyFcyExchangeRate;
								vd27.DMultiCurrencyId = obj.MultiCurrencyId;
								vd27.DExchangeCurrencyRate = obj.ExchangeRate;
								vd27.DCurrencyAmount = ThirdFcyGainLoss4;
								vh.voucherDetailList.Add(vd27);
							}
						}
						else
						{
							VoucherDetail vd17 = new VoucherDetail();
							vd17.AccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd17.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd17.DebitAmount = Conversion.ToDouble(obj.NetAmountRs);
							VoucherAmt += vd17.DebitAmount;
							vd17.CreditAmount = Conversion.ToDouble(0);
							vd17.Comments = obj.Remarks + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount;
							vd17.Comments = vd17.Comments + " @ " + obj.ExchangeRate;
							vd17.DMultiCurrencyId = obj.MultiCurrencyId;
							vd17.DExchangeCurrencyRate = obj.ExchangeRate;
							double FcAmt4 = obj.NetAmountRs / obj.ExchangeRate;
							vd17.DCurrencyAmount = FcAmt4;
							vh.voucherDetailList.Add(vd17);
						}
						double InvoiceFcyAmount = 0.0;
						double ConversionAmount = 0.0;
						double FcyDifferanceAmount = 0.0;
						double DifferanceAmountPKR = 0.0;
						double TotalPartyPKR = 0.0;
						double FcyLadgerBalance = 0.0;
						double FcyLadgerWithoutInvoiceBalance = 0.0;
						double AvgExchangeRate = 0.0;
						DataTable dtResult = GetPartyInvoicesAgainstFIFO(obj);
						if (dtResult != null && dtResult.Rows.Count > 0)
						{
							InvoiceFcyAmount = Conversion.ToDouble(dtResult.Compute("SUM(InvoiceFcyAmount)", ""));
							ConversionAmount = Conversion.ToDouble(dtResult.Compute("SUM(ConversionAmount)", ""));
							if (InvoiceFcyAmount > 0.0 && ConversionAmount > 0.0)
							{
								for (int i = 0; i < dtResult.Rows.Count; i++)
								{
									GainLossBreakup gl11 = new GainLossBreakup();
									gl11.FcyAmount = Conversion.ToDouble(dtResult.Rows[i]["InvoiceFcyAmount"]);
									gl11.LocalAmount = Conversion.ToDouble(dtResult.Rows[i]["ConversionAmount"]);
									gl11.ExchangeRate = Conversion.ToDouble(dtResult.Rows[i]["ExchangeRate"]);
									gl11.Type = "Invoice";
									gl11.Remarks = Conversion.ToString(dtResult.Rows[i]["Remarks"]);
									obj.gainLossBreakupslist.Add(gl11);
									Partyremarks += gl11.Remarks;
								}
							}
						}
						if (obj.FcGrossAmount > 0.0 && InvoiceFcyAmount > 0.0)
						{
							FcyDifferanceAmount = obj.FcGrossAmount - InvoiceFcyAmount;
							if (FcyDifferanceAmount != 0.0)
							{
								ReportsParameters ObjFcy = new ReportsParameters();
								ObjFcy.OrganizationId = obj.OrganizationId;
								ObjFcy.CompanyId = obj.CompanyId;
								ObjFcy.SupplierCustomerId = obj.SupplierCustomerId;
								ObjFcy.FcyId = obj.MultiCurrencyId;
								ObjFcy.ToDate = obj.DocumentDate;
								if (obj.Id > 0)
								{
									ObjFcy.Id = obj.Id;
									ObjFcy.DocumentTypeId = obj.DocumentTypeId;
								}
								DataTable dtFcyBalance = GetFcyAndLcyBalance(ObjFcy);
								if (dtFcyBalance.Rows.Count > 0)
								{
									if (Conversion.ToDouble(dtFcyBalance.Rows[0]["FcyAmount"]) > 0.0)
									{
										FcyLadgerBalance = Conversion.ToDouble(dtFcyBalance.Rows[0]["FcyAmount"]);
										Conversion.ToDouble(dtFcyBalance.Rows[0]["LcyAmount"]);
										AvgExchangeRate = Conversion.ToDouble(dtFcyBalance.Rows[0]["AvgExchangeRate"]);
										FcyLadgerWithoutInvoiceBalance = FcyLadgerBalance - InvoiceFcyAmount;
										if (FcyLadgerWithoutInvoiceBalance >= FcyDifferanceAmount && AvgExchangeRate > 0.0)
										{
											DifferanceAmountPKR = FcyDifferanceAmount * AvgExchangeRate;
											Partyremarks = Partyremarks + " LedgerBalance " + FcyLadgerWithoutInvoiceBalance + " AvgExchangeRate " + AvgExchangeRate;
											GainLossBreakup gl2 = new GainLossBreakup();
											gl2.FcyAmount = Conversion.ToDouble(FcyDifferanceAmount);
											gl2.LocalAmount = Conversion.ToDouble(DifferanceAmountPKR);
											gl2.ExchangeRate = AvgExchangeRate;
											gl2.Type = "LedgerBalance";
											gl2.Remarks = Partyremarks;
											obj.gainLossBreakupslist.Add(gl2);
										}
										else if (FcyLadgerWithoutInvoiceBalance < FcyDifferanceAmount)
										{
											double FcyOtherDiff = 0.0;
											FcyOtherDiff = FcyDifferanceAmount - FcyLadgerWithoutInvoiceBalance;
											DifferanceAmountPKR = FcyLadgerWithoutInvoiceBalance * AvgExchangeRate;
											DifferanceAmountPKR += FcyOtherDiff * obj.ExchangeRate;
											if (FcyLadgerWithoutInvoiceBalance > 0.0)
											{
												Partyremarks = Partyremarks + " LedgerBalance " + FcyLadgerWithoutInvoiceBalance + " AvgExchangeRate " + AvgExchangeRate;
												GainLossBreakup gl4 = new GainLossBreakup();
												gl4.FcyAmount = Conversion.ToDouble(FcyLadgerWithoutInvoiceBalance);
												gl4.LocalAmount = Conversion.ToDouble(FcyLadgerWithoutInvoiceBalance) * AvgExchangeRate;
												gl4.ExchangeRate = AvgExchangeRate;
												gl4.Type = "LedgerBalance";
												gl4.Remarks = Partyremarks;
												obj.gainLossBreakupslist.Add(gl4);
											}
											if (FcyOtherDiff > 0.0)
											{
												Partyremarks = Partyremarks + " ExtraAmount " + FcyOtherDiff + " ExchangeRate " + obj.ExchangeRate;
												GainLossBreakup gl3 = new GainLossBreakup();
												gl3.FcyAmount = Conversion.ToDouble(FcyOtherDiff);
												gl3.LocalAmount = Conversion.ToDouble(FcyOtherDiff) * obj.ExchangeRate;
												gl3.ExchangeRate = obj.ExchangeRate;
												gl3.Type = "ExtraAmount";
												gl3.Remarks = Partyremarks;
												obj.gainLossBreakupslist.Add(gl3);
											}
										}
										else
										{
											DifferanceAmountPKR = FcyDifferanceAmount * obj.ExchangeRate;
										}
									}
									else
									{
										DifferanceAmountPKR = FcyDifferanceAmount * obj.ExchangeRate;
										Partyremarks = Partyremarks + " ExtraAmount " + FcyDifferanceAmount + " ExchangeRate " + obj.ExchangeRate;
										GainLossBreakup gl5 = new GainLossBreakup();
										gl5.FcyAmount = Conversion.ToDouble(FcyDifferanceAmount);
										gl5.LocalAmount = Conversion.ToDouble(DifferanceAmountPKR);
										gl5.ExchangeRate = obj.ExchangeRate;
										gl5.Type = "ExtraAmount";
										gl5.Remarks = Partyremarks;
										obj.gainLossBreakupslist.Add(gl5);
									}
								}
								else
								{
									DifferanceAmountPKR = FcyDifferanceAmount * obj.ExchangeRate;
								}
							}
						}
						else
						{
							ReportsParameters ObjFcy2 = new ReportsParameters();
							ObjFcy2.OrganizationId = obj.OrganizationId;
							ObjFcy2.CompanyId = obj.CompanyId;
							ObjFcy2.SupplierCustomerId = obj.SupplierCustomerId;
							ObjFcy2.FcyId = obj.MultiCurrencyId;
							ObjFcy2.ToDate = obj.DocumentDate;
							if (obj.Id > 0)
							{
								ObjFcy2.Id = obj.Id;
								ObjFcy2.DocumentTypeId = obj.DocumentTypeId;
							}
							DataTable dtFcyBalance2 = GetFcyAndLcyBalance(ObjFcy2);
							if (dtFcyBalance2.Rows.Count > 0)
							{
								if (Conversion.ToDouble(dtFcyBalance2.Rows[0]["FcyAmount"]) > 0.0)
								{
									InvoiceFcyAmount = obj.FcGrossAmount;
									FcyLadgerBalance = Conversion.ToDouble(dtFcyBalance2.Rows[0]["FcyAmount"]);
									Conversion.ToDouble(dtFcyBalance2.Rows[0]["LcyAmount"]);
									AvgExchangeRate = Conversion.ToDouble(dtFcyBalance2.Rows[0]["AvgExchangeRate"]);
									if (FcyLadgerBalance >= obj.FcGrossAmount && AvgExchangeRate > 0.0)
									{
										DifferanceAmountPKR = obj.FcGrossAmount * AvgExchangeRate;
										Partyremarks = Partyremarks + " LedgerBalance " + FcyLadgerBalance + " AvgExchangeRate " + AvgExchangeRate;
										GainLossBreakup gl6 = new GainLossBreakup();
										gl6.FcyAmount = Conversion.ToDouble(obj.FcGrossAmount);
										gl6.LocalAmount = Conversion.ToDouble(DifferanceAmountPKR);
										gl6.ExchangeRate = AvgExchangeRate;
										gl6.Type = "LedgerBalance";
										gl6.Remarks = Partyremarks;
										obj.gainLossBreakupslist.Add(gl6);
									}
									else if (FcyLadgerBalance < obj.FcGrossAmount)
									{
										double FcyOtherDiff2 = 0.0;
										FcyOtherDiff2 = obj.FcGrossAmount - FcyLadgerBalance;
										DifferanceAmountPKR = FcyLadgerBalance * AvgExchangeRate;
										DifferanceAmountPKR += FcyOtherDiff2 * obj.ExchangeRate;
										if (FcyLadgerBalance > 0.0)
										{
											Partyremarks = Partyremarks + " LedgerBalance " + FcyLadgerBalance + " AvgExchangeRate " + AvgExchangeRate;
											GainLossBreakup gl8 = new GainLossBreakup();
											gl8.FcyAmount = Conversion.ToDouble(FcyLadgerBalance);
											gl8.LocalAmount = Conversion.ToDouble(FcyLadgerBalance) * AvgExchangeRate;
											gl8.ExchangeRate = AvgExchangeRate;
											gl8.Type = "LedgerBalance";
											gl8.Remarks = Partyremarks;
											obj.gainLossBreakupslist.Add(gl8);
										}
										if (FcyOtherDiff2 > 0.0)
										{
											GainLossBreakup gl7 = new GainLossBreakup();
											gl7.FcyAmount = Conversion.ToDouble(FcyOtherDiff2);
											gl7.LocalAmount = Conversion.ToDouble(FcyOtherDiff2) * obj.ExchangeRate;
											gl7.ExchangeRate = obj.ExchangeRate;
											gl7.Type = "ExtraAmount";
											gl7.Remarks = Partyremarks;
											obj.gainLossBreakupslist.Add(gl7);
											Partyremarks = Partyremarks + " ExtraAmount " + FcyOtherDiff2 + " ExchangeRate " + obj.ExchangeRate;
										}
									}
									else
									{
										DifferanceAmountPKR = obj.FcGrossAmount * obj.ExchangeRate;
									}
								}
								else
								{
									DifferanceAmountPKR = obj.FcGrossAmount * obj.ExchangeRate;
									Partyremarks = Partyremarks + " ExtraAmount " + obj.FcGrossAmount + " ExchangeRate " + obj.ExchangeRate;
									GainLossBreakup gl9 = new GainLossBreakup();
									gl9.FcyAmount = Conversion.ToDouble(obj.FcGrossAmount);
									gl9.LocalAmount = Conversion.ToDouble(DifferanceAmountPKR);
									gl9.ExchangeRate = obj.ExchangeRate;
									gl9.Type = "ExtraAmount";
									gl9.Remarks = Partyremarks;
									obj.gainLossBreakupslist.Add(gl9);
								}
							}
							else
							{
								DifferanceAmountPKR = obj.FcGrossAmount * obj.ExchangeRate;
								Partyremarks = Partyremarks + " ExtraAmount " + obj.FcGrossAmount + " ExchangeRate " + obj.ExchangeRate;
								GainLossBreakup gl10 = new GainLossBreakup();
								gl10.FcyAmount = Conversion.ToDouble(obj.FcGrossAmount);
								gl10.LocalAmount = Conversion.ToDouble(DifferanceAmountPKR);
								gl10.ExchangeRate = obj.ExchangeRate;
								gl10.Type = "ExtraAmount";
								gl10.Remarks = Partyremarks;
								obj.gainLossBreakupslist.Add(gl10);
							}
						}
						TotalPartyPKR = ConversionAmount + DifferanceAmountPKR;
						GainlossAmt = obj.BankDrAmount + DeductionAmount - TotalPartyPKR;
						double VoucherDifferanceAmount = obj.NetAmountRs - (TotalPartyPKR + GainlossAmt);
						GainlossAmt += VoucherDifferanceAmount;
						if ((obj.TransTypeId == 1 || obj.TransTypeId == 3) && obj.DebitAccountTypeId == 1)
						{
							foreach (FcyPartyPaymentBreackUpForFinancials item4 in obj.FcyPartyPaymentBreackUpForFinancialslist)
							{
								VoucherDetail vd8 = new VoucherDetail();
								vd8.AccountId = vh.RefAccountId;
								vd8.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
								vd8.Comments = obj.Remarks + "   " + obj.CurrencyCode + "  " + item4.RealizedAmount + "  @ " + obj.ExchangeRate + " FBC " + item4.FBC;
								vd8.CreditAmount = Conversion.ToDouble(TotalPartyPKR) / obj.FcGrossAmount * Conversion.ToDouble(item4.RealizedAmount);
								BillAmt += vd8.CreditAmount;
								vd8.DMultiCurrencyId = obj.MultiCurrencyId;
								vd8.DExchangeCurrencyRate = Conversion.ToDouble(TotalPartyPKR) / obj.FcGrossAmount;
								vd8.DCurrencyAmount = Conversion.ToDouble(item4.RealizedAmount);
								vh.voucherDetailList.Add(vd8);
								item4.ExchangeRate = Conversion.ToDecimal(vd8.DExchangeCurrencyRate);
								item4.LcyAmount = Conversion.ToDecimal(vd8.CreditAmount);
							}
							if (obj.FcyPartyPaymentBreackUpForFinancialslist.Count == 0)
							{
								throw new Exception("FcyPartyPaymentBreackUpForFinancialslist not found");
							}
						}
						else
						{
							VoucherDetail vd9 = new VoucherDetail();
							vd9.AccountId = vh.RefAccountId;
							vd9.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd9.Comments = obj.Remarks + "   " + obj.CurrencyCode + "  " + obj.FcGrossAmount + "  @ " + obj.ExchangeRate;
							vd9.DebitAmount = Conversion.ToDouble(0);
							vd9.CreditAmount = Conversion.ToDouble(TotalPartyPKR);
							BillAmt += vd9.CreditAmount;
							vd9.DMultiCurrencyId = obj.MultiCurrencyId;
							vd9.DExchangeCurrencyRate = obj.ExchangeRate;
							vd9.DCurrencyAmount = obj.FcGrossAmount;
							vh.voucherDetailList.Add(vd9);
						}
						if (obj.FcFBCharges > 0.0)
						{
							if (obj.FBCDebitAccountId == 0)
							{
								throw new Exception("FBCDebitAccountId not found");
							}
							VoucherDetail vd34 = new VoucherDetail();
							vd34.AccountId = obj.FBCDebitAccountId;
							vd34.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd34.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
							vd34.DebitAmount = Conversion.ToDouble(obj.FcFBCharges) * obj.ExchangeRate;
							vd34.DMultiCurrencyId = obj.MultiCurrencyId;
							vd34.DExchangeCurrencyRate = obj.ExchangeRate;
							vd34.DCurrencyAmount = obj.FcFBCharges;
							vh.voucherDetailList.Add(vd34);
							VoucherDetail vd30 = new VoucherDetail();
							vd30.AccountId = vh.RefAccountId;
							vd30.AgainstAccountId = Conversion.ToInt(obj.FBCDebitAccountId);
							vd30.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
							vd30.CreditAmount = Conversion.ToDouble(obj.FcFBCharges) * obj.ExchangeRate;
							BillAmt += vd30.CreditAmount;
							vd30.DMultiCurrencyId = obj.MultiCurrencyId;
							vd30.DExchangeCurrencyRate = obj.ExchangeRate;
							vd30.DCurrencyAmount = obj.FcFBCharges;
							vh.voucherDetailList.Add(vd30);
						}
						if (GainlossAmt != 0.0)
						{
							VoucherDetail vd25 = new VoucherDetail();
							vd25.AccountId = obj.VoucherHeadId;
							vd25.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							if (GainlossAmt > 0.0)
							{
								vd25.DebitAmount = Conversion.ToDouble(0);
								vd25.CreditAmount = Conversion.ToDouble(GainlossAmt);
							}
							else
							{
								vd25.CreditAmount = Conversion.ToDouble(0);
								vd25.DebitAmount = Math.Abs(Conversion.ToDouble(GainlossAmt));
							}
							vd25.Comments = Partyremarks;
							vd25.DMultiCurrencyId = obj.MultiCurrencyId;
							vd25.DExchangeCurrencyRate = obj.ExchangeRate;
							if (obj.VoucherExchangeRate > 0.0)
							{
								double FcYAmtGain3 = Math.Abs(GainlossAmt) / obj.VoucherExchangeRate;
								vd25.DCurrencyAmount = FcYAmtGain3;
							}
							else
							{
								double FcYAmtGain4 = Math.Abs(GainlossAmt) / obj.ExchangeRate;
								vd25.DCurrencyAmount = FcYAmtGain4;
							}
							vd25.RateCutAmount = GainlossAmt;
							vh.voucherDetailList.Add(vd25);
						}
					}
					else
					{
						if (obj.DebitAccountTypeId == 2 && obj.ExchangeRate > 0.0 && obj.ThirdCurrencyReceiverExchangeRate > 0.0 && obj.ThirdCurrencyReceiverFcyAmount > 0.0)
						{
							double ReceiverLcyAmount3 = 0.0;
							double ThirdFcyGainLoss3 = 0.0;
							ThirdFcyGainLoss3 = obj.FcGrossAmount - obj.ThirdCurrencyReceiverFcyAmount;
							ReceiverLcyAmount3 = Conversion.ToDouble(obj.ThirdCurrencyReceiverFcyAmount) * obj.ExchangeRate;
							VoucherDetail vd16 = new VoucherDetail();
							vd16.AccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd16.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd16.DebitAmount = ReceiverLcyAmount3;
							VoucherAmt += vd16.DebitAmount;
							vd16.CreditAmount = Conversion.ToDouble(0);
							vd16.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Received From " + PartyName + " @ " + obj.ThirdCurrencyFcyExchangeRate + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount + " @ " + obj.ExchangeRate + " Receiver Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate;
							vd16.DMultiCurrencyId = obj.MultiCurrencyId;
							vd16.DExchangeCurrencyRate = obj.ExchangeRate;
							vd16.DCurrencyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vd16.ThirdCurrencyId = obj.ThirdCurrencyId;
							vd16.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
							vd16.ThirdCurrencyAmount = obj.ThirdCurrencyAmount;
							vd16.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
							vd16.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vh.voucherDetailList.Add(vd16);
							if (ThirdFcyGainLoss3 != 0.0)
							{
								VoucherDetail vd26 = new VoucherDetail();
								vd26.AccountId = obj.VoucherHeadId;
								vd26.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								if (ThirdFcyGainLoss3 > 0.0)
								{
									vd26.DebitAmount = Conversion.ToDouble(ThirdFcyGainLoss3) * obj.ExchangeRate;
								}
								else
								{
									vd26.CreditAmount = Math.Abs(Conversion.ToDouble(ThirdFcyGainLoss3)) * obj.ExchangeRate;
								}
								vd26.Comments = obj.ThirdCurrencyCode + "  " + obj.ThirdCurrencyAmount + " Ex.Rate " + obj.ThirdCurrencyReceiverExchangeRate + "  Party Ex.Rate " + obj.ThirdCurrencyFcyExchangeRate;
								vd26.DMultiCurrencyId = obj.MultiCurrencyId;
								vd26.DExchangeCurrencyRate = obj.ExchangeRate;
								vd26.DCurrencyAmount = ThirdFcyGainLoss3;
								vh.voucherDetailList.Add(vd26);
							}
						}
						else
						{
							VoucherDetail vd15 = new VoucherDetail();
							vd15.AccountId = Conversion.ToInt(obj.BankGlDrAc);
							vd15.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd15.DebitAmount = Conversion.ToDouble(obj.NetAmountRs);
							VoucherAmt += vd15.DebitAmount;
							vd15.CreditAmount = Conversion.ToDouble(0);
							vd15.Comments = obj.Remarks + "  " + obj.CurrencyCode + "  " + obj.FcGrossAmount;
							vd15.Comments = vd15.Comments + " @ " + obj.ExchangeRate;
							vd15.DMultiCurrencyId = obj.MultiCurrencyId;
							vd15.DExchangeCurrencyRate = obj.ExchangeRate;
							double FcAmt3 = obj.NetAmountRs / obj.ExchangeRate;
							vd15.DCurrencyAmount = FcAmt3;
							vd15.ThirdCurrencyId = obj.DebitAccountCurrencyId;
							vd15.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
							vd15.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd15.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
							vd15.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
							vd15.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
							vh.voucherDetailList.Add(vd15);
						}
						VoucherDetail vd10 = new VoucherDetail();
						vd10.AccountId = vh.RefAccountId;
						vd10.AgainstAccountId = Conversion.ToInt(obj.BankGlDrAc);
						vd10.Comments = obj.Remarks + "   " + obj.CurrencyCode + "  " + obj.FcGrossAmount + "  @ " + obj.ExchangeRate;
						vd10.DebitAmount = Conversion.ToDouble(0);
						vd10.CreditAmount = Conversion.ToDouble(obj.NetAmountRs);
						BillAmt += vd10.CreditAmount;
						vd10.DMultiCurrencyId = obj.MultiCurrencyId;
						vd10.DExchangeCurrencyRate = obj.ExchangeRate;
						double FcYAmt2 = obj.NetAmountRs / obj.ExchangeRate;
						vd10.DCurrencyAmount = FcYAmt2;
						vd10.ThirdCurrencyId = obj.ThirdCurrencyId;
						vd10.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
						vd10.ThirdCurrencyAmount = ((obj.ThirdCurrencyAmount > 0.0 && obj.ThirdCurrencyFcyExchangeRate > 0.0) ? (vd10.DCurrencyAmount * obj.ThirdCurrencyFcyExchangeRate) : obj.ThirdCurrencyAmount);
						vd10.ThirdCurrencyReceiverExchangeRate = obj.ThirdCurrencyReceiverExchangeRate;
						vd10.ThirdCurrencyReceiverFcyAmount = obj.ThirdCurrencyReceiverFcyAmount;
						vh.voucherDetailList.Add(vd10);
						if (obj.FcFBCharges > 0.0)
						{
							if (obj.FBCDebitAccountId == 0)
							{
								throw new Exception("FBCDebitAccountId not found");
							}
							VoucherDetail vd35 = new VoucherDetail();
							vd35.AccountId = obj.FBCDebitAccountId;
							vd35.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd35.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
							vd35.DebitAmount = Conversion.ToDouble(obj.FcFBCharges) * obj.ExchangeRate;
							vd35.DMultiCurrencyId = obj.MultiCurrencyId;
							vd35.DExchangeCurrencyRate = obj.ExchangeRate;
							vd35.DCurrencyAmount = obj.FcFBCharges;
							vh.voucherDetailList.Add(vd35);
							VoucherDetail vd31 = new VoucherDetail();
							vd31.AccountId = vh.RefAccountId;
							vd31.AgainstAccountId = Conversion.ToInt(obj.FBCDebitAccountId);
							vd31.Comments = "Foreign Bank Charges " + obj.FcFBCharges;
							vd31.CreditAmount = Conversion.ToDouble(obj.FcFBCharges) * obj.ExchangeRate;
							BillAmt += vd31.CreditAmount;
							vd31.DMultiCurrencyId = obj.MultiCurrencyId;
							vd31.DExchangeCurrencyRate = obj.ExchangeRate;
							vd31.DCurrencyAmount = obj.FcFBCharges;
							vd31.ThirdCurrencyId = obj.ThirdCurrencyId;
							vd31.ThirdCurrencyFcyExchangeRate = obj.ThirdCurrencyFcyExchangeRate;
							vd31.ThirdCurrencyAmount = ((obj.ThirdCurrencyAmount > 0.0 && obj.ThirdCurrencyFcyExchangeRate > 0.0) ? (vd31.DCurrencyAmount * obj.ThirdCurrencyFcyExchangeRate) : obj.ThirdCurrencyAmount);
							vh.voucherDetailList.Add(vd31);
						}
					}
					if (obj.FcBankReceiptDetail.Count > 0 && (!(obj.ExmLcPaymentTerm == "Multi Invoices Payment") || (obj.TransTypeId != 2 && obj.TransTypeId != 3) || obj.DebitAccountTypeId != 1))
					{
						foreach (ExImFcBankReceiptsDeductions r in obj.FcBankReceiptDetail.ToList())
						{
							if (dtCharges.Count > 0)
							{
								List<Architecture.Model.Export.ExImProceedsChargesType> find2 = dtCharges.Where((Architecture.Model.Export.ExImProceedsChargesType x) => x.Id == Conversion.ToInt(r.ExImProceedsChargesTypeId)).ToList();
								if (find2.Count <= 0)
								{
									throw new Exception("Charges AccountType GLAccountId not Found");
								}
								VoucherDetail vd2 = new VoucherDetail();
								vd2.AccountId = Conversion.ToInt(find2[0].ProceedsChargesGlAccount);
								vd2.AgainstAccountId = obj.BankGlDrAc;
								if (r.ProceedsRemarks == string.Empty)
								{
									vd2.Comments = r.ProceedsRemarks + "  " + find2[0].ProceedsChargesTypedescription;
								}
								else
								{
									vd2.Comments = r.ProceedsRemarks;
								}
								vd2.DebitAmount = Conversion.ToDouble(r.RsAmount);
								VoucherAmt += vd2.DebitAmount;
								if (r.Type == "Local")
								{
									double FYAmount2 = r.RsAmount / obj.ExchangeRate;
									vd2.DCurrencyAmount = FYAmount2;
									vd2.DExchangeCurrencyRate = obj.ExchangeRate;
									vd2.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								else
								{
									vd2.DCurrencyAmount = r.FcAmount;
									vd2.DExchangeCurrencyRate = obj.ExchangeRate;
									vd2.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								vd2.CreditAmount = Conversion.ToDouble(0);
								vh.voucherDetailList.Add(vd2);
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = obj.BankGlDrAc;
								vd5.AgainstAccountId = Conversion.ToInt(find2[0].ProceedsChargesGlAccount);
								if (r.ProceedsRemarks == string.Empty)
								{
									vd5.Comments = r.ProceedsRemarks + "  " + find2[0].ProceedsChargesTypedescription;
								}
								else
								{
									vd5.Comments = r.ProceedsRemarks;
								}
								vd5.DebitAmount = Conversion.ToDouble(0);
								vd5.CreditAmount = Conversion.ToDouble(r.RsAmount);
								BillAmt += vd5.CreditAmount;
								if (r.Type == "Local")
								{
									double FcYAmount2 = r.RsAmount / obj.ExchangeRate;
									vd5.DCurrencyAmount = FcYAmount2;
									vd5.DExchangeCurrencyRate = obj.ExchangeRate;
									vd5.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								else
								{
									vd5.DCurrencyAmount = r.FcAmount;
									vd5.DExchangeCurrencyRate = obj.ExchangeRate;
									vd5.DMultiCurrencyId = obj.MultiCurrencyId;
								}
								vd5.ThirdCurrencyId = obj.DebitAccountCurrencyId;
								vd5.ThirdCurrencyFcyExchangeRate = obj.DebitAccountCurrencyExchangeRate;
								vd5.ThirdCurrencyAmount = ((obj.DebitAccountCurrencyAmount > 0.0 && obj.DebitAccountCurrencyExchangeRate > 0.0) ? (vd5.DCurrencyAmount * obj.DebitAccountCurrencyExchangeRate) : obj.DebitAccountCurrencyAmount);
								vh.voucherDetailList.Add(vd5);
							}
						}
					}
				}
				vh.VoucherAmount = VoucherAmt;
				vh.BillAmount = BillAmt;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.Export.ExImFCBankReceipts obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.FcBankVoucherHead = MakeVoucher(obj);
				return new Architecture.DAL.Export.ExImFCBankReceipts().SetDate(obj, "Sp_ExImFCBankReceipts_Insert");
			}
			obj.FcBankVoucherHead = MakeVoucher(obj);
			return new Architecture.DAL.Export.ExImFCBankReceipts().SetDate(obj, "Sp_ExImFCBankReceipts_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Export.ExImFCBankReceipts obj)
	{
		try
		{
			new DataTable();
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
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataBySuppliercustomerId(Architecture.Model.Export.ExImFCBankReceipts obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByFCBankReceiptBySupplierCustomerId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Export.ExImFCBankReceipts obj)
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
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_GetAllMethod", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImFCBankReceipts GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImFCBankReceipts>();
			return new Architecture.DAL.Export.ExImFCBankReceipts().GetDate("Sp_ExImFCBankReceipts_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable HistoryForm(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.TranTypes != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransTypeId",
					Value = obj.TranTypes
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
			if (obj.ReceiverAcId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReceiverAccountId",
					Value = obj.ReceiverAcId
				});
			}
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceId",
					Value = obj.InvoiceId
				});
			}
			if (obj.PaymentTerm != null && obj.PaymentTerm != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PaymentTerm",
					Value = obj.PaymentTerm
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("USP_ExImFCBankReceipts_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImFCBankReceipts_AdvancePaymentReceiptsUtilizedRpt(ReportsParameters obj)
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
			if (obj.ExImFCBankReceiptsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImFCBankReceiptsId",
					Value = obj.ExImFCBankReceiptsId
				});
			}
			if (obj.ExImEFormRegistrationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImEFormRegistrationId",
					Value = obj.ExImEFormRegistrationId
				});
			}
			if (obj.BankGlDrAc != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankGlDrAc",
					Value = obj.BankGlDrAc
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
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_AdvancePaymentReceiptsUtilized_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetFcBankReceiptHistory(ReportsParameters obj)
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
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.OrderId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankAccountId",
					Value = obj.AccountId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@ReportType",
				Value = obj.ReportType
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_Register_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetVoucherNo(Architecture.Model.Export.ExImFCBankReceipts obj)
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
				Value = "VoucherNoComboFill"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetReferenceNo(Architecture.Model.Export.ExImFCBankReceipts obj)
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
				Value = "GetReferenceNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("Sp_ExImFCBankReceipts_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFcyAndLcyBalance(ReportsParameters obj)
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
				ParameterName = "@FcyId",
				Value = obj.FcyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("USP_GetFcyAndLcyBalance", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromFcyBankReceipts(ReportsParameters obj)
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("USP_GetDataForDropDownFromFcyBankReceipts", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FcyReceiptsSummaryRegister(ReportsParameters obj)
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
					ParameterName = "@BranchId",
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SenderAccountId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReceiverAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceId",
					Value = obj.InvoiceId
				});
			}
			if (obj.PaymentTerm != null && obj.PaymentTerm != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PaymentTerm",
					Value = obj.PaymentTerm
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("USP_FcyReceiptsSummaryRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FcyBankCharges_Register(ReportsParameters obj)
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
			if (obj.BankId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankId",
					Value = obj.BankId
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChargesAccountId",
					Value = obj.AccountId
				});
			}
			if (obj.FDBCNo != null && obj.FDBCNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FDBCNo",
					Value = obj.FDBCNo
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("[dbo].[usp_FcyBankCharges_Register]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGainAndLossBreackup(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("USP_GetGainAndLossBreackup", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGdBreakUpsByGdId(int Id, int DocumentTypeId)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("USP_GetGdBreakUpsByGdId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getFcyReceiptsPaymentType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImFCBankReceipts>("usp_getFcyReceiptsPaymentType", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingFcyReceiptsAgainstBank(int OrganizationId, int CompanyId, int FinancialYearId)
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
			if (FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = FinancialYearId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.FcyBankReceiptBreakUp>("Usp_GetPendingFcyReceiptsAgainstBank", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
