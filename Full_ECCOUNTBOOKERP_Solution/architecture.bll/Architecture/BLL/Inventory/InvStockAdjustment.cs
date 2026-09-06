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

namespace Architecture.BLL.Inventory;

public class InvStockAdjustment
{
	public static VoucherHead MakeVoucher(Architecture.Model.Inventory.InvStockAdjustment obj)
	{
		List<Architecture.Model.Inventory.GetItemGlIdsandItemName> dtitem = new List<Architecture.Model.Inventory.GetItemGlIdsandItemName>();
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
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (obj.InvStockAdjustmentDetailslist.Count > 0)
			{
				string Remarks = string.Empty;
				double TotalAmount = 0.0;
				bool FIFOCgs = false;
				if (obj.DocumentTypeId != 215)
				{
					FIFOCgs = CommonServices.GetERPFeaturesByCompanyId(obj.OrganizationId, obj.CompanyId, 5);
				}
				bool loaderFlag = false;
				int LineId = 1;
				foreach (InvStockAdjustmentDetail item in obj.InvStockAdjustmentDetailslist.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<Architecture.Model.Inventory.GetItemGlIdsandItemName> find = dtitem.Where((Architecture.Model.Inventory.GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0)
						{
							if (item.RefDocumentTypeId > 0 && item.RefDocNoId > 0 && item.RefDocSubIdNo > 0)
							{
								loaderFlag = true;
							}
							string remarks = "Item: " + find[0].ItemName + ",   " + $"Qty: {item.Qty},   " + $"Weight: {item.NetWeight},   " + $"Rate: {item.ItemRate}";
							if (!string.IsNullOrWhiteSpace(item.RateUom))
							{
								remarks = remarks + ", RateUom: " + item.RateUom;
							}
							remarks += $", Item Amount: {item.Amount}";
							if (obj.AdjustmentTypeId == 1)
							{
								VoucherDetail vd4 = new VoucherDetail();
								vd4.LineId = LineId;
								vd4.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd4.AgainstAccountId = Conversion.ToInt(item.CreditAccountId);
								vd4.Comments = Conversion.ToString(Remarks);
								vd4.DebitAmount = Conversion.ToDouble(item.Amount);
								vd4.ItemId = Conversion.ToInt(item.ItemId);
								vd4.QtyIn = Conversion.ToDouble(item.Qty);
								vd4.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd4.WeightIn = Conversion.ToDouble(item.NetWeight);
								vd4.ItemAmount = Conversion.ToDouble(item.Amount);
								vd4.BranchesId = obj.BranchesId;
								vh.voucherDetailList.Add(vd4);
								VoucherDetail vd6 = new VoucherDetail();
								vd6.LineId = LineId;
								vd6.AccountId = Conversion.ToInt(item.CreditAccountId);
								vd6.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd6.Comments = Conversion.ToString(Remarks);
								vd6.CreditAmount = Conversion.ToDouble(item.Amount);
								vd6.ItemId = Conversion.ToInt(item.ItemId);
								vd6.QtyIn = Conversion.ToDouble(item.Qty);
								vd6.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd6.WeightIn = Conversion.ToDouble(item.NetWeight);
								vd6.ItemAmount = Conversion.ToDouble(item.Amount);
								vd6.BranchesId = obj.BranchesId;
								vh.voucherDetailList.Add(vd6);
								LineId++;
								TotalAmount += vd6.CreditAmount;
							}
							if (obj.AdjustmentTypeId == 2 && (!FIFOCgs || loaderFlag))
							{
								VoucherDetail vd3 = new VoucherDetail();
								vd3.LineId = LineId;
								vd3.AccountId = Conversion.ToInt(item.DebitAccountId);
								vd3.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd3.Comments = Conversion.ToString(Remarks);
								vd3.DebitAmount = Conversion.ToDouble(item.Amount);
								vd3.ItemId = Conversion.ToInt(item.ItemId);
								vd3.QtyOut = Conversion.ToDouble(item.Qty);
								vd3.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd3.WeightOut = Conversion.ToDouble(item.NetWeight);
								vd3.ItemAmount = Conversion.ToDouble(item.Amount);
								vd3.BranchesId = obj.BranchesId;
								vh.voucherDetailList.Add(vd3);
								VoucherDetail vd5 = new VoucherDetail();
								vd5.LineId = LineId;
								vd5.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd5.AgainstAccountId = Conversion.ToInt(item.DebitAccountId);
								vd5.Comments = Conversion.ToString(Remarks);
								vd5.CreditAmount = Conversion.ToDouble(item.Amount);
								vd5.ItemId = Conversion.ToInt(item.ItemId);
								vd5.QtyOut = Conversion.ToDouble(item.Qty);
								vd5.ItemRate = Conversion.ToDouble(item.ItemRate);
								vd5.WeightOut = Conversion.ToDouble(item.NetWeight);
								vd5.ItemAmount = Conversion.ToDouble(item.Amount);
								vd5.BranchesId = obj.BranchesId;
								vh.voucherDetailList.Add(vd5);
								TotalAmount += vd5.CreditAmount;
								LineId++;
							}
						}
						continue;
					}
					throw new Exception("Item Record Not found");
				}
				vh.VoucherAmount = TotalAmount;
				vh.BillAmount = TotalAmount;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return vh;
	}

	public static int Save(Architecture.Model.Inventory.InvStockAdjustment obj)
	{
		try
		{
			bool InventoryFinancialsEffectsInActive = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "InventoryFinancialsEffectsInActive"));
			if (!InventoryFinancialsEffectsInActive)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
			}
			if (obj.Id == 0)
			{
				obj.ModifyUserId = 0;
				return new Architecture.DAL.Inventory.InvStockAdjustment().SetData(obj, "Sp_InvStockAdjustment_Insert", InventoryFinancialsEffectsInActive);
			}
			obj.EnteryUserId = 0;
			return new Architecture.DAL.Inventory.InvStockAdjustment().SetData(obj, "Sp_InvStockAdjustment_Update", InventoryFinancialsEffectsInActive);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvStockAdjustment GetByID(int Id)
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
			new List<Architecture.Model.Inventory.InvStockAdjustment>();
			return new Architecture.DAL.Inventory.InvStockAdjustment().GetDate("Sp_InvStockAdjustment_GetAllMethod", p)[0];
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
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
			if (obj.AdjustmentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AdjustmentTypeId",
					Value = obj.AdjustmentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockAdjustment>("Sp_InvStockAdjustment_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Inventory.InvStockAdjustment obj)
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
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockAdjustment>("Sp_InvStockAdjustment_GetAllMethod", p);
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

	public static DataTable StockAdjustmentSlipAndRegister409(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AdjustmentTypeId",
					Value = obj.ActionId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockAdjustment>("Sp_StockAdjustmentSlipAndRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromStockAdjustment(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != string.Empty || Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SupplierCustomer>("[dbo].[USP_GetDataForDropDownFromStockAdjustment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockEvaludationDataForAdjustment(ReportsParameters obj)
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
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockAdjustment>("USP_GetStockEvaludationDataForAdjustment", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockTransactionsDataForAdjustment(ReportsParameters obj)
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
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategoriesId",
					Value = obj.InventoryParentCategories
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
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvStockAdjustment>("USP_GetStockTransactionsDataForAdjustment", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
