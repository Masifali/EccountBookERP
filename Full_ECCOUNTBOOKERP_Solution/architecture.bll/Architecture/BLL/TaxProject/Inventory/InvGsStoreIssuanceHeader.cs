using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Architecture.BLL.TaxProject.Common;
using Architecture.Common;
using Architecture.DAL.TaxProject.Inventory;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.BLL.TaxProject.Inventory;

public class InvGsStoreIssuanceHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefAccountId = obj.RefDocNoId;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.Remarks);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.DueDate = DateTime.Now;
			vh.DueDays = 0;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if ((obj.DocumentTypeId == 451 || obj.DocumentTypeId == 452 || obj.DocumentTypeId == 213) && obj.gsStoreIssuanceDetails.Count > 0)
			{
				string Remarks = string.Empty;
				foreach (InvGsStoreIssuanceDetail item in obj.gsStoreIssuanceDetails.ToList())
				{
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0)
						{
							VoucherDetail vd2 = new VoucherDetail();
							vd2.LineId = item.LineId;
							Remarks = obj.Remarks + "  Qty: " + item.IssueQty + "   " + Conversion.ToString(find[0].ItemName) + "   Rate:" + item.ItemRate;
							if (obj.DocumentTypeId == 451)
							{
								if (Conversion.ToInt(obj.RefDocNoId) == 0)
								{
									throw new Exception("Please select Debit Account first");
								}
								vd2.AccountId = Conversion.ToInt(obj.RefDocNoId);
								vd2.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							else
							{
								if (item.DrAccountId > 0)
								{
									vd2.AccountId = Conversion.ToInt(item.DrAccountId);
								}
								else
								{
									vd2.AccountId = Conversion.ToInt(find[0].COGSGLAC);
								}
								vd2.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							vd2.Comments = Conversion.ToString(Remarks);
							vd2.DebitAmount = Conversion.ToSingle(item.ItemAmount);
							vd2.CreditAmount = Conversion.ToSingle(0);
							vd2.ItemId = Conversion.ToInt(item.ItemId);
							vd2.QtyOut = Conversion.ToDouble(item.IssueQty);
							vd2.ItemRate = Conversion.ToDouble(item.ItemRate);
							vd2.ItemAmount = Conversion.ToDouble(item.ItemAmount);
							vd2.BranchesId = obj.BranchesId;
							vh.voucherDetailList.Add(vd2);
							VoucherDetail vd3 = new VoucherDetail();
							vd3.LineId = item.LineId;
							if (obj.DocumentTypeId == 451)
							{
								vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
								vd3.AgainstAccountId = Conversion.ToInt(obj.RefDocNoId);
							}
							else
							{
								if (item.DrAccountId > 0)
								{
									vd3.AgainstAccountId = Conversion.ToInt(item.DrAccountId);
								}
								else
								{
									vd3.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
								}
								vd3.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							}
							vd3.Comments = Conversion.ToString(Remarks);
							vd3.CreditAmount = Conversion.ToSingle(item.ItemAmount);
							vd3.DebitAmount = Conversion.ToSingle(0);
							vd3.ItemId = Conversion.ToInt(item.ItemId);
							vd3.QtyOut = Conversion.ToDouble(item.IssueQty);
							vd3.ItemRate = Conversion.ToDouble(item.ItemRate);
							vd3.ItemAmount = Conversion.ToDouble(item.ItemAmount);
							vd3.BranchesId = obj.BranchesId;
							vh.voucherDetailList.Add(vd3);
							continue;
						}
						throw new Exception("Item Purchase GL Account Not found");
					}
					throw new Exception("Item Record Not found");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.TaxProject.Inventory.InvGsStoreIssuanceHeader obj)
	{
		try
		{
			bool status = false;
			DataTable dt = DateLock.GetByOrganizationCompanyIdAndDate(new Architecture.Model.DateLock
			{
				OrganizationId = obj.OrganizationId,
				CompanyId = obj.CompanyId
			});
			if (dt != null && (obj.DocDate == Conversion.ToDateTime(dt.Rows[0]["Date"]) || obj.DocDate < Conversion.ToDateTime(dt.Rows[0]["Date"])))
			{
				throw new Exception("Not Insert or Update record please check lock date");
			}
			List<ConfigrationsAllocation> stockConversion = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "StoreIssuanceFinancialEffect").ToList();
			if (stockConversion.Count > 0)
			{
				status = Conversion.ToBool(stockConversion[0].ConfigKey.ToString());
			}
			if (obj.Id == 0)
			{
				if (status && obj.ActionForVoucherId == 1)
				{
					obj.VoucherHead = MakeVoucher(obj);
				}
				obj.ActionId = 1;
				return new Architecture.DAL.TaxProject.Inventory.InvGsStoreIssuanceHeader().SetData(obj, "Sp_InvGsStoreIssuanceHeader_Insert");
			}
			if (status && obj.ActionForVoucherId == 1)
			{
				obj.VoucherHead = MakeVoucher(obj);
			}
			obj.ActionId = 2;
			return new Architecture.DAL.TaxProject.Inventory.InvGsStoreIssuanceHeader().SetData(obj, "Sp_InvGsStoreIssuanceHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
