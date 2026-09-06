using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.TaxProject.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.DAL.TaxProject.Export;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Export;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Export;

public class ExImInvoice
{
	public static VoucherHead MakeVoucherForExImInvoice(Architecture.Model.TaxProject.Export.ExImInvoice obj)
	{
		int CommissionAgentGlId = 0;
		VoucherHead vh = new VoucherHead();
		try
		{
			List<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName> dtitem = new List<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName>();
			List<Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId> dtSupplier = new List<Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId>();
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocCode);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.RemarksHeader);
			vh.RemarksOtherLingo = "";
			vh.VoucherAmount = obj.EquivalentAmount;
			vh.FcAmount = obj.TotalAmount;
			vh.ExchangeCurrencyRate = obj.ConversionRate;
			vh.MultiCurrencyId = obj.FcurrencyId;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count > 0)
			{
				List<Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId> find3 = dtSupplier.Where((Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find3.Count <= 0)
				{
					throw new Exception("Customer GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find3[0].GlAccountId);
				if (obj.CommissionAgentId > 0)
				{
					List<Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId> findCom = dtSupplier.Where((Architecture.Model.TaxProject.Inventory.SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.CommissionAgentId)).ToList();
					if (findCom.Count <= 0)
					{
						throw new Exception("Commission Agent GLAccountId not Found");
					}
					CommissionAgentGlId = Conversion.ToInt(findCom[0].GlAccountId);
				}
			}
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = obj.TotalAmount;
			vh.ManualBillNo = obj.InvoiceNo;
			vh.DueDate = obj.EFormDate;
			vh.DueDays = 0;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.ActionId = 1;
			if (obj.DocumentTypeId == 204 || obj.DocumentTypeId == 211 || obj.DocumentTypeId == 212)
			{
				double MTon = 0.0;
				double TotalWeight = 0.0;
				double FcyItemAmount = 0.0;
				if (obj.ExImInvoiceDetail.Count > 0 && obj.DocumentTypeId != 212)
				{
					string Remarks2 = "";
					foreach (Architecture.Model.TaxProject.Export.ExImInvoicePackingDetail item2 in obj.ExImInvoiceDetail.ToList())
					{
						if (item2.ActionTypeId == 3)
						{
							continue;
						}
						VoucherDetail vd5 = new VoucherDetail();
						Remarks2 = "   NoOfBags: " + item2.OuterQty + "   M.Ton: " + Conversion.ToDouble(item2.NetWeight / 1000.0) + "  @Rate: " + item2.RatePrice + "  FcAmount:" + item2.FcAmount;
						if (obj.CreditAccountId == 0)
						{
							if (dtitem.Count <= 0)
							{
								throw new Exception("Item Record Not found");
							}
							List<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName> find2 = dtitem.Where((Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
							if (find2.Count <= 0)
							{
								throw new Exception("Item Record Not found");
							}
							obj.CreditAccountId = find2[0].SaleGLAC;
						}
						if (obj.CreditAccountId == 0)
						{
							throw new Exception("CreditAccount Not Found please Check");
						}
						double LocalAmount4 = Conversion.ToDouble(item2.FcAmount) * obj.ConversionRate;
						vd5.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd5.AgainstAccountId = Conversion.ToInt(obj.CreditAccountId);
						vd5.Comments = Conversion.ToString(Remarks2);
						vd5.DebitAmount = Conversion.ToDouble(LocalAmount4);
						vd5.CreditAmount = Conversion.ToDouble(0);
						vd5.ItemId = Conversion.ToInt(item2.ItemId);
						vd5.QtyOut = Conversion.ToDouble(item2.OuterQty);
						vd5.ItemRate = Conversion.ToDouble(item2.RatePrice);
						vd5.WeightOut = Conversion.ToDouble(item2.NetWeight);
						vd5.RateCut = 0.0;
						vd5.RateCutAmount = 0.0;
						vd5.ItemAmount = Conversion.ToDouble(LocalAmount4);
						vd5.DCurrencyAmount = Conversion.ToDouble(item2.FcAmount);
						vd5.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
						vd5.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
						vd5.Freight = 0.0;
						vd5.Expenses = 0.0;
						vd5.Journal = 0.0;
						vd5.JobLotId = 0;
						vd5.Commission = 0.0;
						vd5.OrderNo = 0;
						vd5.GpNo = 0;
						vd5.VehicleNo = "";
						vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd5.DocumentTypeIdRef = 1;
						vd5.RefInvoiceNo = "SALE";
						MTon += item2.MTon;
						TotalWeight += item2.NetWeight;
						FcyItemAmount += vd5.DCurrencyAmount;
						vh.voucherDetailList.Add(vd5);
						VoucherDetail vd9 = new VoucherDetail();
						vd9.AccountId = Conversion.ToInt(obj.CreditAccountId);
						vd9.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd9.Comments = Conversion.ToString(Remarks2);
						vd9.CreditAmount = Conversion.ToDouble(LocalAmount4);
						vd9.DebitAmount = Conversion.ToDouble(0);
						vd9.ItemId = Conversion.ToInt(item2.ItemId);
						vd9.QtyOut = Conversion.ToDouble(item2.OuterQty);
						vd9.ItemRate = Conversion.ToDouble(item2.RatePrice);
						vd9.WeightOut = Conversion.ToDouble(item2.NetWeight);
						vd9.RateCut = 0.0;
						vd9.RateCutAmount = 0.0;
						vd9.ItemAmount = Conversion.ToDouble(LocalAmount4);
						vd9.DCurrencyAmount = Conversion.ToDouble(item2.FcAmount);
						vd9.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
						vd9.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
						vd9.Freight = 0.0;
						vd9.Expenses = 0.0;
						vd9.Journal = 0.0;
						vd9.JobLotId = 0;
						vd9.Commission = 0.0;
						vd9.OrderNo = 0;
						vd9.GpNo = 0;
						vd9.VehicleNo = "";
						vd9.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd9.DocumentTypeIdRef = 1;
						vd9.RefInvoiceNo = "SALE";
						vh.voucherDetailList.Add(vd9);
					}
				}
				if (obj.DocumentTypeId == 212)
				{
					string Remarks = "";
					VoucherDetail vd4 = new VoucherDetail();
					Remarks = " M.Ton: " + Conversion.ToDouble(obj.NetWeight / 1000.0) + " NetWeight: " + obj.NetWeight + " FcAmount: " + obj.FCurrencyAmount + " AddLess: " + obj.AddLessAmount + " TotalFcyAmount: " + obj.TotalAmount;
					if (obj.CreditAccountId == 0)
					{
						throw new Exception("CreditAccount Not Found please Check");
					}
					double LocalAmount3 = Conversion.ToDouble(obj.TotalAmount) * obj.ConversionRate;
					vd4.AccountId = Conversion.ToInt(vh.RefAccountId);
					vd4.AgainstAccountId = Conversion.ToInt(obj.CreditAccountId);
					vd4.Comments = Conversion.ToString(Remarks);
					vd4.DebitAmount = Conversion.ToDouble(LocalAmount3);
					vd4.CreditAmount = Conversion.ToDouble(0);
					vd4.QtyOut = Conversion.ToDouble(obj.NetWeight / 1000.0);
					vd4.WeightOut = Conversion.ToDouble(obj.NetWeight);
					vd4.ItemAmount = Conversion.ToDouble(LocalAmount3);
					vd4.DCurrencyAmount = Conversion.ToDouble(obj.TotalAmount);
					vd4.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
					vd4.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
					vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vd4.DocumentTypeIdRef = 1;
					vd4.RefInvoiceNo = "SALE";
					vh.voucherDetailList.Add(vd4);
					VoucherDetail vd8 = new VoucherDetail();
					vd8.AccountId = Conversion.ToInt(obj.CreditAccountId);
					vd8.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
					vd8.Comments = Conversion.ToString(Remarks);
					vd8.CreditAmount = Conversion.ToDouble(LocalAmount3);
					vd8.QtyOut = Conversion.ToDouble(obj.NetWeight / 1000.0);
					vd8.WeightOut = Conversion.ToDouble(obj.NetWeight);
					vd8.ItemAmount = Conversion.ToDouble(LocalAmount3);
					vd8.DCurrencyAmount = Conversion.ToDouble(obj.TotalAmount);
					vd8.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
					vd8.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
					vd8.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
					vd8.DocumentTypeIdRef = 1;
					vd8.RefInvoiceNo = "SALE";
					vh.voucherDetailList.Add(vd8);
				}
				if ((obj.DocumentTypeId == 211 || obj.DocumentTypeId == 212) && obj.CommissionAmount > 0.0 && obj.CommissionDebitAcId > 0 && CommissionAgentGlId > 0)
				{
					string CommissionRemarks = "";
					CommissionRemarks = "Mton  " + MTon + "  TotalWeight " + TotalWeight + "  Comm% " + obj.CommissionPercentage;
					VoucherDetail vd3 = new VoucherDetail();
					double LocalAmount2 = Conversion.ToDouble(obj.CommissionAmount) * obj.ConversionRate;
					vd3.AccountId = Conversion.ToInt(obj.CommissionDebitAcId);
					vd3.AgainstAccountId = Conversion.ToInt(CommissionAgentGlId);
					vd3.Comments = Conversion.ToString(CommissionRemarks);
					vd3.DebitAmount = Conversion.ToDouble(LocalAmount2);
					vd3.CreditAmount = Conversion.ToDouble(0);
					vd3.ItemAmount = Conversion.ToDouble(LocalAmount2);
					vd3.DCurrencyAmount = Conversion.ToDouble(obj.CommissionAmount);
					vd3.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
					vd3.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
					vd3.DocumentTypeIdRef = 1;
					vd3.RefInvoiceNo = "SALE";
					vh.voucherDetailList.Add(vd3);
					VoucherDetail vd7 = new VoucherDetail();
					vd7.AccountId = Conversion.ToInt(CommissionAgentGlId);
					vd7.AgainstAccountId = Conversion.ToInt(obj.CommissionDebitAcId);
					vd7.Comments = Conversion.ToString(CommissionRemarks);
					vd7.CreditAmount = Conversion.ToDouble(LocalAmount2);
					vd7.DebitAmount = Conversion.ToDouble(0);
					vd7.ItemAmount = Conversion.ToDouble(LocalAmount2);
					vd7.DCurrencyAmount = Conversion.ToDouble(obj.CommissionAmount);
					vd7.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
					vd7.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
					vd7.DocumentTypeIdRef = 1;
					vd7.RefInvoiceNo = "SALE";
					vh.voucherDetailList.Add(vd7);
				}
				if (obj.ExImInvoiceOtherItemslist.Count > 0)
				{
					foreach (Architecture.Model.TaxProject.Export.ExImInvoiceOtherItems item in obj.ExImInvoiceOtherItemslist.ToList())
					{
						VoucherDetail vd2 = new VoucherDetail();
						if (obj.CreditAccountId == 0)
						{
							if (dtitem.Count <= 0)
							{
								throw new Exception("OtherItem Record Not found");
							}
							List<Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.TaxProject.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.oItemAmount)).ToList();
							if (find.Count <= 0)
							{
								throw new Exception("OtherItem Record Not found");
							}
							obj.CreditAccountId = find[0].SaleGLAC;
						}
						if (obj.CreditAccountId == 0)
						{
							throw new Exception("CreditAccount Not Found against Other Item please Check");
						}
						double LocalAmount = Conversion.ToDouble(item.oItemAmount) * obj.ConversionRate;
						vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.AgainstAccountId = Conversion.ToInt(obj.CreditAccountId);
						vd2.Comments = Conversion.ToString(item.OtherItemRemarks);
						vd2.DebitAmount = Conversion.ToDouble(LocalAmount);
						vd2.CreditAmount = Conversion.ToDouble(0);
						vd2.ItemId = Conversion.ToInt(item.otherItemId);
						vd2.QtyOut = Conversion.ToDouble(item.oItemQty);
						vd2.ItemRate = Conversion.ToDouble(item.oItemRate);
						vd2.WeightOut = Conversion.ToDouble(item.oItemQty);
						vd2.ItemAmount = Conversion.ToDouble(LocalAmount);
						vd2.DCurrencyAmount = Conversion.ToDouble(item.oItemAmount);
						vd2.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
						vd2.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
						vd2.DocumentTypeIdRef = 1;
						vd2.RefInvoiceNo = "SALE";
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd6 = new VoucherDetail();
						vd6.AccountId = Conversion.ToInt(obj.CreditAccountId);
						vd6.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd6.Comments = Conversion.ToString(item.OtherItemRemarks);
						vd6.CreditAmount = Conversion.ToDouble(LocalAmount);
						vd6.DebitAmount = Conversion.ToDouble(0);
						vd6.ItemId = Conversion.ToInt(item.otherItemId);
						vd6.QtyOut = Conversion.ToDouble(item.oItemQty);
						vd6.ItemRate = Conversion.ToDouble(item.oItemRate);
						vd6.WeightOut = Conversion.ToDouble(item.oItemQty);
						vd6.ItemAmount = Conversion.ToDouble(LocalAmount);
						vd6.DCurrencyAmount = Conversion.ToDouble(item.oItemAmount);
						vd6.DExchangeCurrencyRate = Conversion.ToDouble(obj.ConversionRate);
						vd6.DMultiCurrencyId = Conversion.ToInt(obj.FcurrencyId);
						vd2.DocumentTypeIdRef = 1;
						vd2.RefInvoiceNo = "SALE";
						vh.voucherDetailList.Add(vd6);
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

	public static int Save(Architecture.Model.TaxProject.Export.ExImInvoice obj)
	{
		try
		{
			if (obj.DocumentTypeId == 204)
			{
				if (Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FinancialisActiveonCommercialInvoice")))
				{
					obj.VoucherHeadExImInvoices = MakeVoucherForExImInvoice(obj);
				}
			}
			else if (obj.DocumentTypeId == 211 || obj.DocumentTypeId == 212)
			{
				obj.VoucherHeadExImInvoices = MakeVoucherForExImInvoice(obj);
			}
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.TaxProject.Export.ExImInvoice().SetData(obj, "Sp_ExImInvoice_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.TaxProject.Export.ExImInvoice().SetData(obj, "Sp_ExImInvoice_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Export.ExImInvoice obj)
	{
		DataTable dt = null;
		try
		{
			int DocNo = 0;
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
				ParameterName = "@Activity",
				Value = "GenerateDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GenerateInvoiceNo(Architecture.Model.Export.ExImInvoice obj)
	{
		DataTable dt = null;
		try
		{
			string InvoiceNo = "";
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateInvoiceNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				InvoiceNo = Conversion.ToString(dt.Rows[0]["InvoiceNo"]);
			}
			return InvoiceNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportInvoice(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetInvoicesDataForLoader", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImInvoice GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImInvoice>();
			return new Architecture.DAL.Export.ExImInvoice().GetData("Sp_ExImInvoice_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImInvoice_ReadByIdForPackingList(int ExImInvoiceId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ExImInvoiceId",
				Value = ExImInvoiceId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("[dbo].[USP_ExImInvoice_ReadByIdForPackingList]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ExImInvoicePackingDetail_ReadByHeaderId(int ExImInvoiceId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ExImInvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadExImInvoicePackingDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("[dbo].[Sp_ExImInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable InvDeliveryOrder_ReadByInvoiceForPackingList(int ExImInvoiceId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ExImInvoiceId",
				Value = ExImInvoiceId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("[dbo].[USP_InvDeliveryOrder_ReadByInvoiceForPackingList]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<Architecture.Model.Export.ExImInvoicePackingListDetail> ExImInvoicePackingListDetail_ReadByInvoiceId(int ExImInvoiceId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ExImInvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ExImInvoicePackingListDetail_ReadByHeaderId"
			});
			return Conversion.ConvertDataTableToList<Architecture.Model.Export.ExImInvoicePackingListDetail>(GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("[dbo].[Sp_ExImInvoice_GetAllMethod]", p));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ExImInvoiceOtherChargesDetail_ReadByHeaderId(int ExImInvoiceId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ExImInvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ExImInvoiceOtherChargesDetail_ReadByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("[dbo].[Sp_ExImInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetInvoiceNo(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "InvoiceNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoandPartiesForFcBankReceipts(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetInvoiceNoandPartiesForFcBankReceipts"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExImInvoicePackingDetailForVouchers(ReportsParameters obj)
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
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("usp_getexportInvoiceDetailByIdForExportVoucher", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFcYAmount(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetFcYAmount"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingEximInvoicesByDoWeight(Architecture.Model.Export.ExImInvoice obj)
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DoId",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PendingEximInvoicesByDoWeight"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoForExportForwarding(Architecture.Model.Export.ExImInvoice obj)
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
				ParameterName = "@GpId",
				Value = obj.Id
			});
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
				Value = "GetPendingInvoicesForExportForwarding"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoForBookingInfo(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetInvoiceNoForBookingInfo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoForBillOfLading(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetInvoiceNoForBillOfLading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceDetailByHeaderId(Architecture.Model.Export.ExImInvoice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportSalesAuditByWeight(ReportsParameters obj)
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
			if (obj.EximInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceId",
					Value = obj.EximInvoiceId
				});
			}
			if (obj.ExImLcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.ExImLcOrderId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@gpDateTo",
					Value = obj.ToDate
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceStatus",
					Value = obj.Status
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("SpEximInvoice_DoWeightWbWeightDiff_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExchangeRateandCurrencyFromVoucher(Architecture.Model.Export.ExImInvoice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicewiseDoWeightandBalanceWeight(Architecture.Model.Export.ExImInvoice obj)
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
				ParameterName = "@ExImInvoiceId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoicewiseDoWeightandBalanceWeight"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_InvDeliveryOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetSalesContractBalanceMton(Architecture.Model.Export.ExImInvoice obj)
	{
		DataTable dt = null;
		try
		{
			double MtonBalance = 0.0;
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
				Value = obj.LcOrderNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSalesContractBalanceMton"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				MtonBalance = Conversion.ToDouble(dt.Rows[0]["BalMton"]);
			}
			return MtonBalance;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvoiceOtherItemsByInvoiceIds(string IDs)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Ids",
				Value = IDs
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ExImInvoiceOtherItemsByInvoiceIds"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetOtherItemByExImInvoiceId(ReportsParameters obj)
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
				Value = "GetOtherItemByExImInvoiceId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetInvoiceDetailByInvoiceIdForDeliveryOrder(ReportsParameters obj)
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
				Value = "GetInvoiceDetailByInvoiceIdForDeliveryOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetInvoiceInformationByInvoiceIdForServices(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetInvoiceInformationByInvoiceIdForServices"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerIdsFromServicesBillbyInvoice(Architecture.Model.Export.ExImInvoice obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierCustomerIdsFromServicesBillbyInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesForExImExportShipingLineBooking(Architecture.Model.Export.ExImInvoice obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoicesForExImExportShipingLineBooking"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFcYAmountByContractId(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetFcYAmountByContractId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataFromInvoicesByDeliveryOrder(Architecture.Model.Export.ExImInvoice obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDataFromInvoicesByDeliveryOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadJobLotsFromShipmentAnalysis(Architecture.Model.Export.ExImInvoice obj)
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
			if (obj.SupplierCustomerId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.LcOrderNoId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.LcOrderNoId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadJobLotsFromShipmentAnalysis"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesForForwarding(Architecture.Model.Export.ExImInvoice obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoicesForForwarding"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesAndCustomers(Architecture.Model.Export.ExImInvoice obj)
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
				Value = "GetInvoicesAndCustomers"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImInvoice_GetInvoicesData(int OrganizationId, int CompanyId, int BranchesId, int FinancialYearId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("[dbo].[UPS_ExImInvoice_GetInvoicesData]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContractsByInvoiceId(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetContractsByInvoiceId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportInvoiceHeaderDataByInvoiceId(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetExportInvoiceHeaderDataByInvoiceId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceDetailForDetailId(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetInvoiceDetailForDetailId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportInvoiceDetailByHeaderId(ReportsParameters obj)
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
				ParameterName = "@ContractId",
				Value = obj.OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ExportInvoiceDetailByHeaderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ExportInvoiceDetailByHeaderIdNew(int OrganizationId, int CompanyId, int ContractId, int ExImInvoiceId)
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
				ParameterName = "@ContractId",
				Value = ContractId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ExImInvoiceId",
				Value = ExImInvoiceId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("[dbo].[USP_ExImInvoice_ExportInvoiceDetailByHeaderId]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetProformaDataForInvoices(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeIds",
				Value = obj.DocumentTypeIds
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
			if (obj.Ids != "" && obj.Ids != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
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
			if (obj.FcyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FcurrencyId",
					Value = obj.FcyId
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
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.ZeroBalanceType
				});
			}
			if (obj.ContractScheduleIds != null && obj.ContractScheduleIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ScheduleIds",
					Value = obj.ContractScheduleIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetProformaDataForInvoices", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetProformaDataForPreInvoices(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeIds",
				Value = obj.DocumentTypeIds
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
			if (obj.Ids != "" && obj.Ids != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
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
			if (obj.FcyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FcurrencyId",
					Value = obj.FcyId
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
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.ZeroBalanceType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetProformaDataForPreInvoices", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportContractDataForPmDetail(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeIds",
				Value = obj.DocumentTypeIds
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
			if (obj.Ids != "" && obj.Ids != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("[dbo].[USP_GetExportContractDataForPmDetail]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPreInvoicesAndForwardingDateForCommercialInvoices(ReportsParameters obj)
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
			if (obj.ExImLcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.ExImLcOrderId
				});
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Ids",
					Value = obj.Ids
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
			if (obj.FcyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FcurrencyId",
					Value = obj.FcyId
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
			if (obj.PackingExpiryDate != null && obj.PackingExpiryDate != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingExpiryDate",
					Value = obj.PackingExpiryDate
				});
			}
			if (obj.ProductionNo != null && obj.ProductionNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductionNo",
					Value = obj.ProductionNo
				});
			}
			if (obj.ZeroBalanceType != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SkipZero",
					Value = obj.ZeroBalanceType
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetPreInvoicesAndForwardingDateForCommercialInvoices", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPreInvoicenumberForCommercialInvoice(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("UPS_GetPreInvoicenumberForCommercialInvoice", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetPreInvoiceDataByPreInvoiceId(int ExImInvoiceId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ExImInvoiceId",
				Value = ExImInvoiceId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("UPS_GetPreInvoiceHeaderDataForCommercialInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetLastCommissionDebitAccountId(int OrganizationId, int CompanyId)
	{
		int CommissionDebitAcId = 0;
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
				Value = "GetLastCommissionDebitAccountId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				CommissionDebitAcId = Conversion.ToInt(dt.Rows[0][0]);
			}
			return CommissionDebitAcId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCommercialInvoicenumberForPackingDetail(ReportsParameters obj)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("UPS_GetCommercialInvoicenumberForPackingDetail", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable FormHistoryForPackingDetailList(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeIds",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
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
				ParameterName = "@Activity",
				Value = "FormHistoryForPackingDetailList"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("Sp_ExImInvoice_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCommercialInvoicesaginstPreInvoices(Architecture.Model.Export.ExImInvoice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetCommercialInvoicesaginstPreInvoices", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FinancialInstrumentsForDropDown(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("usp_GetFinancialInstrumentsForDropDown", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportInvoicesFromVouchers(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetExportInvoicesFromVouchers]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFcyBankInvoicesBalance(Architecture.Model.Export.ExImInvoice obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("USP_GetFcyBankInvoicesBalance", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesDataForShipmentAnalysis(Architecture.Model.Export.ExImInvoice obj)
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
			if (obj.LcOrderNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.LcOrderNoId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("[dbo].[USP_GetInvoicesDataForShipmentAnalysis]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContractSchedulesDataForShipmentAnalysis(Architecture.Model.Export.ExImInvoice obj)
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
			if (obj.LcOrderNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.LcOrderNoId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("[dbo].[USP_GetContractSchedulesDataForShipmentAnalysis]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContainerNoFromShipingLineBookingDetailByInvoiceId(int OrganizationId, int CompanyId, int InvoiceId, int DoId = 0)
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
				ParameterName = "@InvoiceId",
				Value = InvoiceId
			});
			if (DoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DoId",
					Value = DoId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImInvoice>("[dbo].[USP_GetContainerNoFromShipingLineBookingDetailByInvoiceId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateContractNoInvoiceWise(int ContractId)
	{
		try
		{
			int No = 0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractId",
				Value = ContractId
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("USP_GenerateContractNoInvoiceWise", p);
			if (dt.Rows.Count > 0)
			{
				No = Conversion.ToInt(dt.Rows[0]["NoofShimpment"]);
			}
			return No;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBankInvoicesForGdBreakUp(int OrganizationId, int CompanyId, int Id = 0)
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
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			return GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("USP_GetBankInvoicesForGdBreakUp", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBankInvoicesForGdBreakUpNew(int OrganizationId, int CompanyId, int Id = 0)
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
			if (Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			return GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("USP_GetBankInvoicesForGdBreakUpNew", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveGdBreakUp(ExImInvoiceBankGDBreakUp obj)
	{
		try
		{
			return new Architecture.DAL.Export.ExImInvoice().SetDataGdBreakUp(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static ExImInvoiceBankGDBreakUp GetGdBreakUpByInvoiceId(int InvoiceId)
	{
		try
		{
			ExImInvoiceBankGDBreakUp obj = new ExImInvoiceBankGDBreakUp();
			obj.ExImInvoiceBankGDBreakUpslist = new List<ExImInvoiceBankGDBreakUp>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = InvoiceId
			});
			DataTable dt = GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("USP_ExImInvoiceBankGDBreakUp_ReadById", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				obj.ExImInvoiceBankGDBreakUpslist = Conversion.ConvertDataTableToList<ExImInvoiceBankGDBreakUp>(dt);
			}
			return obj;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesForAdvanceUtilize(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("USP_GetInvoicesForAdvanceUtilize", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImInvoiceBankGDBreakUp_History(ReportsParameters obj)
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
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate ",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate  ",
					Value = obj.ToDate
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BankId ",
					Value = obj.AccountId
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId ",
					Value = obj.DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("[dbo].[USP_ExImInvoiceBankGDBreakUp_History]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGDsAgainstAdvancePaymentUtilizeInInvoice(int OrganizationId, int CompanyId, int Id = 0, int RecId = 0, int RefDocumentTypeId = 0)
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
			if (Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = Id
				});
			}
			if (RecId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = RecId
				});
			}
			if (RefDocumentTypeId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = RefDocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("USP_GetGDsAgainstAdvancePaymentUtilizeInInvoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingFINosForAdvanceUtilize(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<ExImInvoiceBankGDBreakUp>("USP_GetPendingFINosForAdvanceUtilize", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromExportGdBreakup(int OrganizationId, int CompanyId, string Activity = null)
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
				Value = Activity
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetDataForDropDownFromExportGDBreakUps]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGdBalanceFromBankRequest(ReportsParameters obj)
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
				ParameterName = "@BankId ",
				Value = obj.AccountId
			});
			if (obj.Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId  ",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetGdBalanceFromBankRequest]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveAdvanceUtilize(AdvanceUtilizeagainstGDs obj)
	{
		try
		{
			return new Architecture.DAL.Export.ExImInvoice().SetDataAdvanceUtilize(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static AdvanceUtilizeagainstGDs GetAdvanceUtilizeByInvoiceId(int InvoiceId, int RefDocumentTypeId = 0)
	{
		try
		{
			AdvanceUtilizeagainstGDs obj = new AdvanceUtilizeagainstGDs();
			obj.AdvanceUtilizeagainstGDslist = new List<AdvanceUtilizeagainstGDs>();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = InvoiceId
			});
			if (RefDocumentTypeId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = RefDocumentTypeId
				});
			}
			DataTable dt = GenericProvider.GetDataTableProc<AdvanceUtilizeagainstGDs>("USP_AdvanceUtilizeagainstGDs_ReadById", p);
			if (dt != null && dt.Rows.Count > 0)
			{
				obj.AdvanceUtilizeagainstGDslist = Conversion.ConvertDataTableToList<AdvanceUtilizeagainstGDs>(dt);
			}
			return obj;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AdvanceUtilizeagainstGDs_FormHistory(int OrganizationId, int CompanyId)
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
			return GenericProvider.GetDataTableProc<AdvanceUtilizeagainstGDs>("usp_AdvanceUtilizeagainstGDs_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommercialInvoiceShipments(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("usp_CommercialInvoice_Shipments", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CommissionAgentFcyLedger(ReportsParameters obj)
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
			Conversion.CheckDateTimeNull(obj.FromDate);
			Conversion.CheckDateTimeNull(obj.ToDate);
			if (obj.CommissionAgentId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CommissionAgentId",
					Value = obj.CommissionAgentId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("usp_CommissionAgentFcyLedger", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ShipmentdataForBrokeryTax(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("usp_ShipmentdataForBrokeryTax", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
