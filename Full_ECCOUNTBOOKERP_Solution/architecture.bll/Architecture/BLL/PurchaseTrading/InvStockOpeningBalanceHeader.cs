using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.PurchaseTrading;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.PurchaseTrading;

namespace Architecture.BLL.PurchaseTrading;

public class InvStockOpeningBalanceHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.Remarks);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = 0.0;
			vh.ManualBillNo = Conversion.ToString(obj.TransactionType);
			vh.DueDate = obj.DocDate;
			vh.DueDays = 0;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUserId;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUserId;
			vh.voucherDetailList = new List<VoucherDetail>();
			Architecture.Model.Inventory.Item objitm = new Architecture.Model.Inventory.Item();
			objitm = Architecture.BLL.Inventory.Item.GetByID(obj.ItemId);
			if (Conversion.ToInt(obj.ItemId) != 0)
			{
				if (objitm == null)
				{
					throw new Exception("Item Record not found");
				}
				JobLot objlot = new JobLot();
				if (obj.JobLotId > 0)
				{
					objlot = jobLot.GetByID(obj.JobLotId);
				}
				string Remarks = "";
				if (obj.Remarks != string.Empty)
				{
					Remarks = obj.Remarks;
				}
				string itemName = clsGlobalVariables.getGlobalAllItems.FirstOrDefault((getGlobalAllItems rr) => rr.Id == obj.ItemId)?.ItemName;
				Remarks = Remarks + "   Item: " + itemName + ",   Qty: " + obj.Qty + ",   Rate:" + obj.ItemRate + ",   Amount:" + obj.ItemAmount;
				VoucherDetail vd = new VoucherDetail();
				if (objlot != null && objlot.AccountId > 0)
				{
					vd.AccountId = objlot.AccountId;
				}
				else
				{
					vd.AccountId = Conversion.ToInt(objitm.PurchaseGLAC);
				}
				vd.AgainstAccountId = Conversion.ToInt(obj.StockCrGLAcId);
				vd.Comments = Remarks;
				vd.DebitAmount = Conversion.ToDouble(obj.ItemAmount);
				vd.CreditAmount = Conversion.ToDouble(0);
				vd.ItemRate = Conversion.ToDouble(obj.ItemRate);
				vd.QtyIn = Conversion.ToDouble(obj.Qty);
				vd.WeightIn = Conversion.ToDouble(obj.WeightKgs);
				vd.ItemAmount = Conversion.ToDouble(obj.ItemAmount);
				vh.voucherDetailList.Add(vd);
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = Conversion.ToInt(obj.StockCrGLAcId);
				if (objlot != null && objlot.AccountId > 0)
				{
					vd2.AgainstAccountId = objlot.AccountId;
				}
				else
				{
					vd2.AgainstAccountId = Conversion.ToInt(objitm.PurchaseGLAC);
				}
				vd2.Comments = Remarks;
				vd2.CreditAmount = Conversion.ToDouble(obj.ItemAmount);
				vd2.DebitAmount = Conversion.ToDouble(0);
				vd2.ItemRate = Conversion.ToDouble(obj.ItemRate);
				vd2.QtyIn = Conversion.ToDouble(obj.Qty);
				vd2.WeightIn = Conversion.ToDouble(obj.WeightKgs);
				vd2.ItemAmount = Conversion.ToDouble(obj.ItemAmount);
				vh.voucherDetailList.Add(vd2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.ModifyUserId = 0;
				obj.VoucherHeadOpeningBalance = MakeVoucher(obj);
				return new Architecture.DAL.PurchaseTrading.InvStockOpeningBalanceHeader().SetData(obj, "Sp_InvStockOpeningBalanceHeader_Insert");
			}
			obj.EntryUserId = 0;
			obj.VoucherHeadOpeningBalance = MakeVoucher(obj);
			obj.PreviousDate = GetRecordsById(obj.Id);
			return new Architecture.DAL.PurchaseTrading.InvStockOpeningBalanceHeader().SetData(obj, "Sp_InvStockOpeningBalanceHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
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
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader>("Sp_InvStockOpeningBalanceHeader_GetAllMethod", p);
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

	public static DataTable GetAllOrById(Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
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
			if (obj.BaseDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BaseDocumentTypeId",
					Value = obj.BaseDocumentTypeId
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
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNo",
					Value = obj.DocNo
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader>("Sp_InvStockOpeningBalanceHeader_GetAllMethod", p);
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
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
				Value = "ReadAll"
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BaseDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BaseDocumentTypeId",
					Value = obj.BaseDocumentTypeId
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
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNo",
					Value = obj.DocNo
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
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
			if (obj.StockAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemStockAccountId",
					Value = obj.StockAccountId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.PurchaseTrading.InvStockOpeningBalanceHeader>("Sp_InvStockOpeningBalanceHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DateTime GetRecordsById(int Id)
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
				Value = "GetPreviousDateById"
			});
			return Conversion.ToDateTime(GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvPurchaseInvoice>("Sp_InvStockOpeningBalanceHeader_GetAllMethod", p).Rows[0][0]);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromInvStockOpeningBalanceHeader(ReportsParameters obj)
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_GetDataForDropDownFromInvStockOpeningBalanceHeader]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
