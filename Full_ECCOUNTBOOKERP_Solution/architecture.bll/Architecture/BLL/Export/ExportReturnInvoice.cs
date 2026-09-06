using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Export;

public class ExportReturnInvoice
{
	public static VoucherHead MakeVoucher(Architecture.Model.Export.ExportReturnInvoice obj)
	{
		List<Architecture.Model.Inventory.SupplierCustomer> dtSupplier = new List<Architecture.Model.Inventory.SupplierCustomer>();
		List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = new List<Architecture.Model.Inventory.GetItemGlIdsandItemName>();
		VoucherHead vh = new VoucherHead();
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
				Value = "ReadByOrganizationCompanyIdForExport"
			});
			dtSupplier = new Architecture.DAL.Inventory.SupplierCustomer().GetData("Sp_SupplierCustomer_GetAllMethod", p);
			if (dtSupplier.Count > 0)
			{
				List<Architecture.Model.Inventory.SupplierCustomer> find2 = dtSupplier.Where((Architecture.Model.Inventory.SupplierCustomer x) => x.Id == Conversion.ToInt(obj.SupplierCustomerId)).ToList();
				if (find2.Count <= 0)
				{
					throw new Exception("Customer GLAccountId not Found");
				}
				vh.RefAccountId = Conversion.ToInt(find2[0].GlAccountId);
			}
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.ExImInvoiceId;
			vh.VoucherCode = obj.DocNo;
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = obj.RemarksHeader;
			vh.RemarksOtherLingo = "";
			vh.VoucherAmount = Conversion.ToDouble(obj.LcyAmount);
			vh.BillAmount = Conversion.ToDouble(obj.LcyAmount);
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.ManualBillNo = obj.InvoiceNo;
			vh.DueDate = DateTime.Now;
			vh.DueDays = 0;
			vh.ExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
			vh.FcAmount = Conversion.ToDouble(obj.FcyAmount);
			vh.MultiCurrencyId = obj.CurrencyId;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.PostUser = 0;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonRepository.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			bool num = obj.ExportReturnInvoiceDetailList.Any((ExportReturnInvoiceDetail x) => x.GrnId == 0);
			int ExportReturnStockInTransitAccount = 0;
			bool SaleCostingJobOrderWise = false;
			if (num)
			{
				ExportReturnStockInTransitAccount = Conversion.ToInt(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ExportReturnStockInTransitAccount"));
				SaleCostingJobOrderWise = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SaleCostingJobOrderWise"));
			}
			string Remarks = string.Empty;
			if (obj.RemarksHeader != string.Empty)
			{
				Remarks = obj.RemarksHeader;
			}
			Remarks = Remarks + " InvoiceNo " + obj.InvoiceNo;
			int LineId = 1;
			foreach (ExportReturnInvoiceDetail item in obj.ExportReturnInvoiceDetailList.ToList())
			{
				if (item.ActionTypeId == 3)
				{
					continue;
				}
				if (dtitem.Count > 0)
				{
					List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item.ItemId)).ToList();
					if (find.Count > 0)
					{
						item.ItemName = Conversion.ToString(find[0].ItemName);
						item.LineId = LineId;
						VoucherDetail vd2 = new VoucherDetail();
						Remarks = Remarks + " Item: " + Conversion.ToString(find[0].ItemName) + " Qty: " + item.Qty + " Weight: " + item.NetWeight + " " + obj.CurrencyName + " @Rate: " + item.RatePrice + " ExchangeRate " + obj.ExchangeRate;
						if (obj.DebitAccounId > 0)
						{
							vd2.AccountId = Conversion.ToInt(obj.DebitAccounId);
						}
						else
						{
							vd2.AccountId = Conversion.ToInt(find[0].SaleGLAC);
						}
						vd2.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.Comments = Conversion.ToString(Remarks);
						vd2.DebitAmount = Conversion.ToDouble(item.LcyAmount);
						vd2.ItemId = Conversion.ToInt(item.ItemId);
						vd2.QtyIn = Conversion.ToDouble(item.Qty);
						vd2.ItemRate = Conversion.ToDouble(item.RatePrice) * Conversion.ToDouble(obj.ExchangeRate);
						vd2.WeightIn = Conversion.ToDouble(item.NetWeight);
						vd2.ItemAmount = Conversion.ToDouble(item.LcyAmount);
						vd2.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd2.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd2.DCurrencyAmount = Conversion.ToDouble(item.FcyAmount);
						vd2.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						vd2.LineId = item.LineId;
						vh.voucherDetailList.Add(vd2);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
						if (obj.DebitAccounId > 0)
						{
							vd3.AgainstAccountId = Conversion.ToInt(obj.DebitAccounId);
						}
						else
						{
							vd3.AgainstAccountId = Conversion.ToInt(find[0].SaleGLAC);
						}
						vd3.Comments = Conversion.ToString(Remarks);
						vd3.CreditAmount = Conversion.ToDouble(item.LcyAmount);
						vd3.ItemId = Conversion.ToInt(item.ItemId);
						vd3.QtyIn = Conversion.ToDouble(item.Qty);
						vd3.ItemRate = Conversion.ToDouble(item.RatePrice) * Conversion.ToDouble(obj.ExchangeRate);
						vd3.WeightIn = Conversion.ToDouble(item.NetWeight);
						vd3.ItemAmount = Conversion.ToDouble(item.LcyAmount);
						vd3.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
						vd3.DMultiCurrencyId = Conversion.ToInt(obj.CurrencyId);
						vd3.DCurrencyAmount = Conversion.ToDouble(item.FcyAmount);
						vd3.DExchangeCurrencyRate = Conversion.ToDouble(obj.ExchangeRate);
						vd3.LineId = item.LineId;
						vh.voucherDetailList.Add(vd3);
						if (item.GrnId == 0)
						{
							if (ExportReturnStockInTransitAccount <= 0)
							{
								throw new Exception("Export Return Stock InTransit Account not map in configuration please check...");
							}
							if (SaleCostingJobOrderWise)
							{
								ReportsParameters parameters = new ReportsParameters();
								parameters.OrganizationId = obj.OrganizationId;
								parameters.CompanyId = obj.CompanyId;
								parameters.ItemId = item.ItemId;
								parameters.JobLotId = item.JobLotId;
								parameters.InvoiceId = item.ExImInvoiceId;
								parameters.InvoiceDetailId = item.ExImInvoiceDetailId;
								item.CostRate = Conversion.ToDecimal(CommonServies.getCostRateFromExportVoucherByInvoice(parameters));
							}
							if (item.CostRate > 0m)
							{
								double CgsAmount = Conversion.ToDouble(item.StockWeight) * Conversion.ToDouble(item.CostRate);
								item.CostAmount = Conversion.ToDecimal(CgsAmount);
								string RemarksCgs = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item.Qty + ",   Weight: " + item.StockWeight + ",   Rate:" + item.CostRate + "  Item Amount: " + CgsAmount;
								VoucherDetail vd6 = new VoucherDetail();
								vd6.AccountId = Conversion.ToInt(ExportReturnStockInTransitAccount);
								vd6.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
								vd6.Comments = Conversion.ToString(RemarksCgs);
								vd6.DebitAmount = Conversion.ToDouble(CgsAmount);
								vd6.ItemId = Conversion.ToInt(item.ItemId);
								vd6.JobLotId = Conversion.ToInt(item.JobLotId);
								vd6.QtyIn = Conversion.ToDouble(item.Qty);
								vd6.WeightIn = Conversion.ToDouble(item.StockWeight);
								vd6.ItemRate = Conversion.ToDouble(item.CostRate);
								vd6.ItemCgsRate = Conversion.ToDouble(item.CostRate);
								vd6.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd6.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd6.RefDocSubIdNo = Conversion.ToInt(item.Id);
								vd6.IsCGS = 1;
								vd6.LineId = item.LineId;
								vh.voucherDetailList.Add(vd6);
								VoucherDetail vd4 = new VoucherDetail();
								vd4.AccountId = Conversion.ToInt(find[0].COGSGLAC);
								vd4.AgainstAccountId = ExportReturnStockInTransitAccount;
								vd4.Comments = Conversion.ToString(RemarksCgs);
								vd4.CreditAmount = Conversion.ToDouble(CgsAmount);
								vd4.ItemId = Conversion.ToInt(item.ItemId);
								vd4.JobLotId = Conversion.ToInt(item.JobLotId);
								vd4.QtyIn = Conversion.ToDouble(item.Qty);
								vd4.WeightIn = Conversion.ToDouble(item.StockWeight);
								vd4.ItemRate = Conversion.ToDouble(item.CostRate);
								vd4.ItemCgsRate = Conversion.ToDouble(item.CostRate);
								vd4.ItemAmount = Conversion.ToDouble(CgsAmount);
								vd4.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd4.RefDocSubIdNo = Conversion.ToInt(item.Id);
								vd4.IsCGS = 1;
								vd4.LineId = item.LineId;
								vh.voucherDetailList.Add(vd4);
							}
						}
						else
						{
							ReportsParameters parameters2 = new ReportsParameters();
							parameters2.OrganizationId = obj.OrganizationId;
							parameters2.CompanyId = obj.CompanyId;
							parameters2.ItemId = item.ItemId;
							parameters2.JobLotId = item.JobLotId;
							parameters2.InvoiceId = item.ExImInvoiceId;
							parameters2.InvoiceDetailId = item.ExImInvoiceDetailId;
							parameters2.WarehouseId = item.WarehouseId;
							parameters2.ItemUomId = item.PackUomId;
							parameters2.CropYearId = item.CropYearId;
							parameters2.PackingTypeId = item.PackingTypeId;
							item.CostRate = Conversion.ToDecimal(CommonServies.getCostRateFromExportVoucherByInvoice(parameters2));
							if (item.CostRate <= 0m)
							{
								throw new Exception("CGS Rate not found against " + Conversion.ToString(find[0].ItemName));
							}
							if (item.CostRate > 0m)
							{
								double CgsAmount2 = Conversion.ToDouble(item.StockWeight) * Conversion.ToDouble(item.CostRate);
								item.CostAmount = Conversion.ToDecimal(CgsAmount2);
								string RemarksCgs2 = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + item.Qty + ",   Weight: " + item.StockWeight + ",   Rate:" + item.CostRate + "  Item Amount: " + CgsAmount2;
								VoucherDetail vd7 = new VoucherDetail();
								vd7.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd7.AgainstAccountId = ((obj.DebitAccounId > 0) ? obj.DebitAccounId : Conversion.ToInt(find[0].COGSGLAC));
								vd7.Comments = Conversion.ToString(RemarksCgs2);
								vd7.DebitAmount = Conversion.ToDouble(CgsAmount2);
								vd7.ItemId = Conversion.ToInt(item.ItemId);
								vd7.JobLotId = Conversion.ToInt(item.JobLotId);
								vd7.QtyIn = Conversion.ToDouble(item.Qty);
								vd7.WeightIn = Conversion.ToDouble(item.StockWeight);
								vd7.ItemRate = Conversion.ToDouble(item.CostRate);
								vd7.ItemCgsRate = Conversion.ToDouble(item.CostRate);
								vd7.ItemAmount = Conversion.ToDouble(CgsAmount2);
								vd7.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd7.RefDocSubIdNo = Conversion.ToInt(item.Id);
								vd7.IsCGS = 1;
								vd7.LineId = item.LineId;
								vh.voucherDetailList.Add(vd7);
								VoucherDetail vd5 = new VoucherDetail();
								vd5.AccountId = ((obj.DebitAccounId > 0) ? obj.DebitAccounId : Conversion.ToInt(find[0].COGSGLAC));
								vd5.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd5.Comments = Conversion.ToString(RemarksCgs2);
								vd5.CreditAmount = Conversion.ToDouble(CgsAmount2);
								vd5.ItemId = Conversion.ToInt(item.ItemId);
								vd5.JobLotId = Conversion.ToInt(item.JobLotId);
								vd5.QtyIn = Conversion.ToDouble(item.Qty);
								vd5.WeightIn = Conversion.ToDouble(item.StockWeight);
								vd5.ItemRate = Conversion.ToDouble(item.CostRate);
								vd5.ItemCgsRate = Conversion.ToDouble(item.CostRate);
								vd5.ItemAmount = Conversion.ToDouble(CgsAmount2);
								vd5.SupplierCustomerId = Conversion.ToInt(obj.SupplierCustomerId);
								vd5.RefDocSubIdNo = Conversion.ToInt(item.Id);
								vd5.IsCGS = 1;
								vd5.LineId = item.LineId;
								vh.voucherDetailList.Add(vd5);
							}
						}
						LineId++;
						continue;
					}
					throw new Exception("ItemGlAccount Not found");
				}
				throw new Exception("ItemGlAccount Not found");
			}
			return vh;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int Save(Architecture.Model.Export.ExportReturnInvoice obj)
	{
		try
		{
			obj.VoucherHeadForInvoice = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				obj.ModifyUserId = 0;
				return new Architecture.DAL.Export.ExportReturnInvoice().SetData(obj, "[dbo].[USP_ExportReturnInvoice_InsertAndUpdate]");
			}
			obj.ActionId = 2;
			obj.EntryUserId = 0;
			return new Architecture.DAL.Export.ExportReturnInvoice().SetData(obj, "[dbo].[USP_ExportReturnInvoice_InsertAndUpdate]");
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (obj.DebitAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DebitAccountId",
					Value = obj.DebitAccountId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.PurchaseOrder>("[dbo].[USP_ExportReturnInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExportReturnInvoice ReadById(int Id)
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
			new List<Architecture.Model.Export.ExportReturnInvoice>();
			return new Architecture.DAL.Export.ExportReturnInvoice().GetDate("[dbo].[USP_ExportReturnInvoice_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int OrganizationId, int CompanyId, int DocumentTypeId, int EntryUserId, int Id)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
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
			GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportReturnInvoice>("[dbo].[USP_ExportReturnInvoice_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Export.ExportReturnInvoice obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			return Conversion.ToInt(GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportReturnInvoice>("[dbo].[USP_ExportReturnInvoice_GetAllMethod]", p).Rows[0]["DocNo"]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromExportReturnInvoice(int OrganizationId, int CompanyId, string DocumentTypeIds = null, string SupplierCustomerIds = null, string Activity = null)
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
			if (DocumentTypeIds != null && DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = DocumentTypeIds
				});
			}
			if (SupplierCustomerIds != null && SupplierCustomerIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerIds",
					Value = SupplierCustomerIds
				});
			}
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportReturnInvoice>("[dbo].[USP_GetDataForDropDownFromExportReturnInvoice]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportReturnInvoiceSlipandRegister(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.TransporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransporterId",
					Value = obj.TransporterId
				});
			}
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (obj.LcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LCOrderId",
					Value = obj.LcOrderId
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
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
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
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceId",
					Value = obj.Id
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExportReturnInvoice>("[dbo].[USp_ExportReturnInvoice_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable ExportRetunrInvoice_GetDataForGrnReturn(ReportsParameters obj)
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
					ParameterName = "@Todate",
					Value = obj.ToDate
				});
			}
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (obj.LcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractId",
					Value = obj.LcOrderId
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
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			return GenericProvider.GetDataTableProc<PurchaseOrderDetail>("usp_ExportRetunrInvoice_GetDataForGrnReturn", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApprovedRecordById(ApprovalList obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (ApprovalList item in obj.AllApprovalLists)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "[usp_ExportReturnInvoice_ApprovedRecordById]";
					sqlCommand.Parameters.AddWithValue("@OrganizationId", item.OrganizationId);
					sqlCommand.Parameters.AddWithValue("@CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("@Id", item.Id);
					sqlCommand.Parameters.AddWithValue("@UserId", item.EntryUser);
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}
}
