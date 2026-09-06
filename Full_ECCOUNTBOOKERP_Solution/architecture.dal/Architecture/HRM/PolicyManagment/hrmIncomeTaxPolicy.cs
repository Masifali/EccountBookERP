using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;

namespace Architecture.HRM.PolicyManagment;

public class hrmIncomeTaxPolicy : DALBase
{
	public int SetData(Architecture.Model.Inventory.InvSaleInvoice obj, string ProcName)
	{
		int StockAgainstAcId = 0;
		int VoucherHeadId = 0;
		int VoucherHeadIdGet = 0;
		List<Architecture.Model.Inventory.Item> dtitem = new List<Architecture.Model.Inventory.Item>();
		List<Architecture.Model.Inventory.SupplierCustomer> dtSupplier = new List<Architecture.Model.Inventory.SupplierCustomer>();
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvSaleInvoiceDetail item8 in obj.InvSaleInvoiceDetaillist)
			{
				item8.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_InvSaleInvoiceDetail_Insert"));
			}
			foreach (InvSaleInvoiceFreight item7 in obj.InvSaleInvoiceFreightList)
			{
				item7.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_InvSaleInvoiceFreight_Insert"));
			}
			foreach (InvSaleInvoiceJournal item6 in obj.InvSaleInvoiceJournalList)
			{
				item6.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_InvSaleInvoiceJournal_Insert"));
			}
			foreach (InvSaleInvoiceExpense item5 in obj.InvSaleInvoiceExpenseList)
			{
				item5.InvSaleInvoiceId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_InvSaleInvoiceExpense_Insert"));
			}
			if (returnID > 0)
			{
				VoucherHead vh = new VoucherHead();
				List<SqlParameter> s = new List<SqlParameter>();
				s.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = clsGlobalVariables.UserAccount.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = clsGlobalVariables.UserAccount.CompanyId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeSrNo",
					Value = returnID
				});
				DataTable dtvh = GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_GetMethods", s);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = (vh.Id = Conversion.ToInt(dtvh.Rows[0]["Id"]));
				}
				vh.DocumentTypeId = obj.DocumentTypeId;
				vh.DocumentTypeSrNo = returnID;
				vh.RefDocNoId = returnID;
				vh.VoucherCode = Conversion.ToInt(obj.DocNo);
				vh.VoucherDate = obj.DocDate;
				vh.Remarks = obj.RemarksHeader.ToString();
				vh.RemarksOtherLingo = "";
				vh.VoucherAmount = obj.BillAmount;
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = clsGlobalVariables.UserAccount.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = clsGlobalVariables.UserAccount.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "ReadByOrganizationCompanyId"
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
				}
				vh.ChequeDate = DateTime.Today;
				vh.IncludeWHT = false;
				vh.BranchId = obj.BranchesId;
				vh.ProjectId = obj.ProjectsId;
				vh.BillAmount = obj.BillAmount;
				vh.ManualBillNo = obj.ManualBillNo;
				vh.DueDate = obj.SupplierInvoiceDate;
				vh.DueDays = Conversion.ToInt(obj.SupplierInvoiceNo);
				vh.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				vh.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
				vh.EntryDate = DateTime.Now;
				vh.ModifyDate = DateTime.Now;
				vh.ModifyUser = clsGlobalVariables.UserAccount.ID;
				vh.PostDate = DateTime.Now;
				vh.PostUser = 0;
				vh.voucherDetailList = new List<VoucherDetail>();
				List<ConfigrationsAllocation> List = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "StockAgainstAccount").ToList();
				if (List.Count > 0)
				{
					StockAgainstAcId = Conversion.ToInt(List[0].ConfigKey.ToString());
					if (StockAgainstAcId <= 0)
					{
						throw new Exception("StockAgainstId not found");
					}
					List<SqlParameter> x2 = new List<SqlParameter>();
					x2.Add(new SqlParameter
					{
						ParameterName = "@OrganizationId",
						Value = clsGlobalVariables.UserAccount.OrganizationId
					});
					x2.Add(new SqlParameter
					{
						ParameterName = "@CompanyId",
						Value = clsGlobalVariables.UserAccount.CompanyId
					});
					x2.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "ReadByOrganizationCompanyId"
					});
					dtitem = new Architecture.DAL.Inventory.Item().GetData("Sp_Item_GetAllMethod", x2);
					new List<SqlParameter>
					{
						new SqlParameter
						{
							ParameterName = "@Activity",
							Value = "ReadAll"
						},
						new SqlParameter
						{
							ParameterName = "@organizationId",
							Value = clsGlobalVariables.UserAccount.OrganizationId
						},
						new SqlParameter
						{
							ParameterName = "@CompanyId",
							Value = clsGlobalVariables.UserAccount.CompanyId
						}
					};
					if (obj.InvSaleInvoiceFreightList.Count > 0)
					{
						foreach (InvSaleInvoiceFreight r in obj.InvSaleInvoiceFreightList.ToList())
						{
							if (Conversion.ToInt(r.TansporterId) != 0)
							{
								VoucherDetail vd3 = new VoucherDetail();
								vd3.AccountId = r.TansporterId;
								vd3.AgainstAccountId = StockAgainstAcId;
								vd3.Comments = "FredightAmount";
								vd3.DebitAmount = 0.0;
								vd3.CreditAmount = Conversion.ToDouble(r.FreightAmount);
								vh.voucherDetailList.Add(vd3);
							}
						}
					}
					if (obj.InvSaleInvoiceJournalList.Count > 0)
					{
						foreach (InvSaleInvoiceJournal item4 in obj.InvSaleInvoiceJournalList.ToList())
						{
							if (Conversion.ToInt(item4.ChartofAccountId) != 0)
							{
								VoucherDetail vd2 = new VoucherDetail();
								vd2.AccountId = Conversion.ToInt(item4.ChartofAccountId);
								vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
								vd2.Comments = Conversion.ToString(item4.JvRemarks);
								vd2.DebitAmount = Conversion.ToDouble(item4.JvDebit);
								vd2.CreditAmount = Conversion.ToDouble(item4.JvCredit);
								vh.voucherDetailList.Add(vd2);
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
								vd5.AgainstAccountId = Conversion.ToInt(item4.ChartofAccountId);
								vd5.Comments = Conversion.ToString(item4.JvRemarks);
								vd5.CreditAmount = Conversion.ToDouble(item4.JvDebit);
								vd5.DebitAmount = Conversion.ToDouble(item4.JvCredit);
								vh.voucherDetailList.Add(vd5);
							}
						}
					}
					string comments = "";
					if (obj.DocumentTypeId == 95)
					{
						if (obj.CommAmount > 0.0)
						{
							if (dtSupplier.Count > 0)
							{
								List<Architecture.Model.Inventory.SupplierCustomer> find2 = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomer x) => x.Id == Conversion.ToInt(obj.CommissionAgentId)).ToList();
								if (find2.Count <= 0)
								{
									throw new Exception("Commission Agent GLAccountId not Found");
								}
								vh.AgainstAccountId = Conversion.ToInt(find2[0].GlAccountId);
							}
							VoucherDetail vd8 = new VoucherDetail();
							vd8.AccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd8.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
							vd8.Comments = Conversion.ToString(obj.CommissionRemarks);
							vd8.DebitAmount = Conversion.ToSingle(0);
							vd8.CreditAmount = Conversion.ToSingle(obj.CommAmount);
							vh.voucherDetailList.Add(vd8);
						}
						if (obj.InvSaleInvoiceExpenseList.Count > 0)
						{
							List<ConfigrationsAllocation> lst2 = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
							if (lst2.Count > 0)
							{
								if (!bool.Parse(lst2[0].ConfigKey))
								{
									double ExpAmount = 0.0;
									foreach (InvSaleInvoiceExpense item2 in obj.InvSaleInvoiceExpenseList.ToList())
									{
										ExpAmount += item2.Amount;
										comments = comments + "/ " + item2.Remarks;
									}
									if (ExpAmount > 0.0)
									{
										VoucherDetail vd4 = new VoucherDetail();
										vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
										vd4.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
										vd4.Comments = Conversion.ToString(comments);
										vd4.DebitAmount = ExpAmount;
										vd4.CreditAmount = 0.0;
										vh.voucherDetailList.Add(vd4);
									}
								}
								else
								{
									double ExpAmount2 = 0.0;
									foreach (InvSaleInvoiceExpense item3 in obj.InvSaleInvoiceExpenseList.ToList())
									{
										ExpAmount2 += item3.Amount;
										comments = comments + "/ " + item3.Remarks;
									}
									VoucherDetail vd = new VoucherDetail();
									vd.AccountId = Conversion.ToInt(vh.RefAccountId);
									vd.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
									vd.Comments = Conversion.ToString(comments);
									vd.DebitAmount = ExpAmount2;
									vh.voucherDetailList.Add(vd);
								}
							}
						}
					}
					if (obj.DocumentTypeId == 95 && obj.InvSaleInvoiceDetaillist.Count > 0)
					{
						double ExpenseAmount = 0.0;
						double FreightAmount = 0.0;
						double CommissionAmount = 0.0;
						double ItemAmount = 0.0;
						double ItemQty = 0.0;
						double NetBillweight = 0.0;
						double RateCut = 0.0;
						double RateCutAmount = 0.0;
						double ItemRate = 0.0;
						int GLAccountId = 0;
						int ItemIdDt = 0;
						int itemId = 0;
						int PurchaseOrderNo = 0;
						string Remarks = string.Empty;
						foreach (InvSaleInvoiceDetail item9 in obj.InvSaleInvoiceDetaillist.ToList())
						{
							if (dtitem.Count > 0)
							{
								List<Architecture.Model.Inventory.Item> find = dtitem.Where((Architecture.Model.Inventory.Item x) => x.Id == Conversion.ToInt(item9.ItemId)).ToList();
								if (find.Count > 0)
								{
									VoucherDetail vd6 = new VoucherDetail();
									PurchaseOrderNo = item9.SaleOrder;
									if (itemId == 0)
									{
										itemId = Conversion.ToInt(item9.ItemId);
									}
									if (itemId == Conversion.ToInt(item9.ItemId))
									{
										ExpenseAmount += item9.ExpenseAmount;
										FreightAmount += item9.FreightAmount;
										CommissionAmount += item9.CommissionAmount;
										ItemAmount += item9.ItemAmount;
										ItemQty += item9.ItemQty;
										NetBillweight += item9.NetBillWeight;
										RateCut += item9.RateCut;
										RateCutAmount += item9.RateCutAmount;
										ItemIdDt = Conversion.ToInt(item9.ItemId);
										GLAccountId = Conversion.ToInt(find[0].SaleGLAC);
										Remarks = "Rate:" + ItemRate + "Qty: " + ItemQty + "Net Weight: " + NetBillweight + "Item Amount: " + ItemAmount + "Ratecut Amount: " + RateCutAmount + "Expense Amount: " + ExpenseAmount + "Commission Amount: " + CommissionAmount;
										continue;
									}
									vd6.AccountId = Conversion.ToInt(find[0].SaleGLAC);
									vd6.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
									vd6.Comments = Conversion.ToString(Remarks);
									vd6.CreditAmount = Conversion.ToSingle(ItemAmount);
									vd6.DebitAmount = Conversion.ToSingle(0);
									vd6.ItemId = Conversion.ToInt(item9.ItemId);
									vd6.QtyOut = Conversion.ToDouble(ItemQty);
									if (NetBillweight > 0.0 && ItemAmount > 0.0)
									{
										ItemRate = ItemAmount / NetBillweight * 40.0;
									}
									vd6.ItemRate = Conversion.ToDouble(ItemRate);
									vd6.WeightOut = Conversion.ToDouble(NetBillweight);
									vd6.RateCut = Conversion.ToDouble(RateCut);
									vd6.RateCutAmount = Conversion.ToDouble(RateCutAmount);
									vd6.ItemAmount = Conversion.ToDouble(ItemAmount);
									vd6.Expenses = Conversion.ToDouble(ExpenseAmount);
									vd6.Commission = Conversion.ToDouble(CommissionAmount);
									vd6.Freight = Conversion.ToDouble(FreightAmount);
									vd6.OrderNo = Conversion.ToInt(PurchaseOrderNo);
									vd6.GpNo = Conversion.ToInt(item9.GpNo);
									vd6.VehicleNo = item9.VehicleNo;
									vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd6);
									VoucherDetail vd7 = new VoucherDetail();
									vd7.AccountId = Conversion.ToInt(vh.RefAccountId);
									vd7.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
									vd7.Comments = Conversion.ToString(Remarks);
									vd7.DebitAmount = Conversion.ToSingle(ItemAmount);
									vd7.CreditAmount = Conversion.ToSingle(0);
									vd7.ItemId = Conversion.ToInt(item9.ItemId);
									vd7.QtyIn = Conversion.ToDouble(ItemQty);
									vd7.ItemRate = Conversion.ToDouble(ItemRate);
									vd7.WeightIn = Conversion.ToDouble(NetBillweight);
									vd7.RateCut = Conversion.ToDouble(RateCut);
									vd7.RateCutAmount = Conversion.ToDouble(RateCutAmount);
									vd7.ItemAmount = Conversion.ToDouble(ItemAmount);
									vd7.Expenses = Conversion.ToDouble(ExpenseAmount);
									vd7.Freight = Conversion.ToDouble(FreightAmount);
									vd7.Commission = Conversion.ToDouble(CommissionAmount);
									vd7.OrderNo = Conversion.ToInt(PurchaseOrderNo);
									vd7.GpNo = Conversion.ToInt(item9.GpNo);
									vd7.VehicleNo = item9.VehicleNo;
									vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
									vh.voucherDetailList.Add(vd7);
									List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
									if (list.Count > 0 && bool.Parse(list[0].ConfigKey) && ExpenseAmount > 0.0)
									{
										VoucherDetail vd10 = new VoucherDetail();
										vd10.AccountId = Conversion.ToInt(find[0].SaleGLAC);
										vd10.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
										vd10.CreditAmount = Conversion.ToDouble(ExpenseAmount);
										vd10.DebitAmount = Conversion.ToDouble(0);
										vh.voucherDetailList.Add(vd10);
									}
									if (CommissionAmount > 0.0)
									{
										VoucherDetail vd14 = new VoucherDetail();
										vd14.AccountId = Conversion.ToInt(find[0].SaleGLAC);
										vd14.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
										vd14.Comments = Conversion.ToString(obj.CommissionRemarks);
										vd14.DebitAmount = Conversion.ToDouble(CommissionAmount);
										vd14.CreditAmount = Conversion.ToDouble(0);
										vh.voucherDetailList.Add(vd14);
									}
									if (FreightAmount > 0.0)
									{
										VoucherDetail vd13 = new VoucherDetail();
										vd13.AccountId = Conversion.ToInt(find[0].SaleGLAC);
										vd13.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
										vd13.Comments = "FreightAmount";
										vd13.DebitAmount = Conversion.ToDouble(FreightAmount);
										vd13.CreditAmount = Conversion.ToDouble(0);
										vh.voucherDetailList.Add(vd13);
									}
									ExpenseAmount = 0.0;
									FreightAmount = 0.0;
									CommissionAmount = 0.0;
									ItemAmount = 0.0;
									itemId = 0;
									ItemQty = 0.0;
									NetBillweight = 0.0;
									RateCut = 0.0;
									RateCutAmount = 0.0;
									ExpenseAmount += item9.ExpenseAmount;
									FreightAmount += item9.FreightAmount;
									CommissionAmount += item9.CommissionAmount;
									ItemAmount += item9.ItemAmount;
									ItemQty += item9.ItemQty;
									NetBillweight += item9.NetBillWeight;
									RateCut += item9.RateCut;
									RateCutAmount += item9.RateCutAmount;
									itemId = Conversion.ToInt(item9.ItemId);
									continue;
								}
								throw new Exception("Item Sale GL Account Not found");
							}
							throw new Exception("Item Record Not found");
						}
						if (ItemAmount > 0.0)
						{
							VoucherDetail vd15 = new VoucherDetail();
							vd15.AccountId = GLAccountId;
							vd15.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd15.Comments = Conversion.ToString(Remarks);
							vd15.CreditAmount = Conversion.ToSingle(ItemAmount);
							vd15.DebitAmount = Conversion.ToSingle(0);
							vd15.ItemId = Conversion.ToInt(ItemIdDt);
							vd15.QtyOut = Conversion.ToDouble(ItemQty);
							if (NetBillweight > 0.0 && ItemAmount > 0.0)
							{
								ItemRate = ItemAmount / NetBillweight * 40.0;
							}
							vd15.ItemRate = Conversion.ToDouble(ItemRate);
							vd15.WeightOut = Conversion.ToDouble(NetBillweight);
							vd15.RateCut = Conversion.ToDouble(RateCut);
							vd15.RateCutAmount = Conversion.ToDouble(RateCutAmount);
							vd15.ItemAmount = Conversion.ToDouble(ItemAmount);
							vd15.Expenses = Conversion.ToDouble(ExpenseAmount);
							vd15.Commission = Conversion.ToDouble(CommissionAmount);
							vd15.OrderNo = Conversion.ToInt(PurchaseOrderNo);
							vd15.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd15);
							VoucherDetail vd16 = new VoucherDetail();
							vd16.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd16.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
							vd16.Comments = Conversion.ToString(Remarks);
							vd16.DebitAmount = Conversion.ToSingle(ItemAmount);
							vd16.CreditAmount = Conversion.ToSingle(0);
							vd16.ItemId = Conversion.ToInt(ItemIdDt);
							vd16.QtyIn = Conversion.ToDouble(ItemQty);
							vd16.ItemRate = Conversion.ToDouble(ItemRate);
							vd16.WeightIn = Conversion.ToDouble(NetBillweight);
							vd16.RateCut = Conversion.ToDouble(RateCut);
							vd16.RateCutAmount = Conversion.ToDouble(RateCutAmount);
							vd16.ItemAmount = Conversion.ToDouble(ItemAmount);
							vd16.Expenses = Conversion.ToDouble(ExpenseAmount);
							vd16.Commission = Conversion.ToDouble(CommissionAmount);
							vd16.OrderNo = Conversion.ToInt(PurchaseOrderNo);
							vd16.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
							vh.voucherDetailList.Add(vd16);
						}
						List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
						if (lst.Count > 0 && bool.Parse(lst[0].ConfigKey) && ExpenseAmount > 0.0)
						{
							VoucherDetail vd9 = new VoucherDetail();
							vd9.AccountId = Conversion.ToInt(GLAccountId);
							vd9.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd9.CreditAmount = Conversion.ToDouble(ExpenseAmount);
							vd9.DebitAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd9);
						}
						if (CommissionAmount > 0.0)
						{
							VoucherDetail vd12 = new VoucherDetail();
							vd12.AccountId = Conversion.ToInt(GLAccountId);
							vd12.AgainstAccountId = Conversion.ToInt(vh.AgainstAccountId);
							vd12.Comments = Conversion.ToString(obj.CommissionRemarks);
							vd12.DebitAmount = Conversion.ToDouble(CommissionAmount);
							vd12.CreditAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd12);
						}
						if (FreightAmount > 0.0)
						{
							VoucherDetail vd11 = new VoucherDetail();
							vd11.AccountId = Conversion.ToInt(GLAccountId);
							vd11.AgainstAccountId = Conversion.ToInt(StockAgainstAcId);
							vd11.Comments = "FreightAmount";
							vd11.DebitAmount = Conversion.ToDouble(FreightAmount);
							vd11.CreditAmount = Conversion.ToDouble(0);
							vh.voucherDetailList.Add(vd11);
						}
					}
					VoucherHeadId = ((VoucherHeadIdGet != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, vh, "Sp_VoucherHead_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, vh, "Sp_VoucherHead_Insert")));
					if (VoucherHeadId > 0)
					{
						vh.Id = VoucherHeadId;
					}
					else
					{
						VoucherHeadId = vh.Id;
					}
					foreach (VoucherDetail item in vh.voucherDetailList)
					{
						item.VoucherHeadId = vh.Id;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_VoucherDetail_Insert"));
					}
				}
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Inventory.InvSaleInvoice> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.InvSaleInvoice> lst = new List<Architecture.Model.Inventory.InvSaleInvoice>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.InvSaleInvoice>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "SaleDetailReadByInvSaleInvoiceId"));
						item4.InvSaleInvoiceDetaillist = GenericProvider.Get<InvSaleInvoiceDetail>(sqlConnection2, "Sp_InvSaleInvoice_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "InvSaleInvoiceExpense_ReadBySaleInvoiceID"));
						item3.InvSaleInvoiceExpenseList = GenericProvider.Get<InvSaleInvoiceExpense>(sqlConnection3, "Sp_InvSaleInvoice_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Inventory.InvSaleInvoice item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "InvSaleInvoiceFreight_ReadBySaleInvoiceID"));
						item2.InvSaleInvoiceFreightList = GenericProvider.Get<InvSaleInvoiceFreight>(sqlConnection4, "Sp_InvSaleInvoice_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Inventory.InvSaleInvoice item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "InvSaleInvoiceJournal_ReadBySaleInvoiceID"));
					item.InvSaleInvoiceJournalList = GenericProvider.Get<InvSaleInvoiceJournal>(sqlcon, "Sp_InvSaleInvoice_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
