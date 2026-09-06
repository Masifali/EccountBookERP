using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc.Production;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.pcc.Production;

namespace Architecture.BLL.pcc.Production;

public class ConversionHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.pcc.Production.ConversionHeader obj)
	{
		VoucherHead voucherHead = new VoucherHead();
		double voucheramt = 0.0;
		try
		{
			List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			voucherHead.DocumentTypeSrNo = obj.Id;
			voucherHead.VoucherDate = obj.DocDate;
			voucherHead.VoucherCode = obj.DocNo;
			voucherHead.DocumentTypeId = obj.DocumentTypeId;
			voucherHead.EntryDate = DateTime.Now;
			voucherHead.ModifyDate = DateTime.Now;
			voucherHead.EntryUser = obj.EntryUserId;
			voucherHead.ModifyUser = obj.ModifyUserId;
			voucherHead.OrganizationId = obj.OrganizationId;
			voucherHead.CompanyId = obj.CompanyId;
			voucherHead.FinancialYearId = obj.FinancialYearId;
			voucherHead.BranchId = obj.BranchesId;
			voucherHead.IsApproved = obj.IsApproved;
			voucherHead.voucherDetailList = new List<VoucherDetail>();
			int LineId = 1;
			foreach (ConversionOutPutDetail item3 in obj.ConversionOutPutDetailList.ToList())
			{
				string Remarks2 = "";
				if (item3.ActionTypeId != 3)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find3 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item3.ItemId)).ToList();
					if (find3.Count > 0)
					{
						Remarks2 = ((!(item3.Remarks == string.Empty)) ? (item3.Remarks + " ItemName: " + find3[0].ItemName + " Qty  " + Conversion.ToDouble(item3.NetWeight) + " Rate  " + Conversion.ToDouble(item3.Rate)) : ("ItemName: " + find3[0].ItemName + " Qty  " + Conversion.ToDouble(item3.NetWeight) + " Rate  " + Conversion.ToDouble(item3.Rate)));
						VoucherDetail vd2 = new VoucherDetail();
						vd2.LineId = LineId;
						vd2.AccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd2.AgainstAccountId = Conversion.ToInt(find3[0].PurchaseGLAC);
						vd2.Comments = Conversion.ToString(Remarks2);
						vd2.DebitAmount = Conversion.ToDouble(item3.Amount);
						vd2.QtyIn = Conversion.ToDouble(item3.ItemQty);
						vd2.WeightIn = Conversion.ToDouble(item3.NetWeight);
						vd2.ItemRate = Conversion.ToDouble(item3.Rate);
						vd2.ItemAmount = Conversion.ToDouble(item3.Amount);
						voucherHead.voucherDetailList.Add(vd2);
						voucheramt += vd2.DebitAmount;
						LineId++;
					}
				}
			}
			LineId = 1;
			foreach (ConversionInputDetail item2 in obj.ConversionInputDetailList.ToList())
			{
				string Remarks = "";
				if (item2.ActionTypeId != 3)
				{
					if (dtitem.Count <= 0)
					{
						throw new Exception("Item GL Account Not Found");
					}
					List<GetItemGlIdsandItemName> find2 = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item2.ItemId)).ToList();
					if (find2.Count > 0)
					{
						Remarks = ((!(item2.Remarks == string.Empty)) ? (item2.Remarks + " ItemName: " + find2[0].ItemName + " Qty  " + Conversion.ToDouble(item2.NetWeight) + " Rate  " + Conversion.ToDouble(item2.Rate)) : ("ItemName: " + find2[0].ItemName + " Qty  " + Conversion.ToDouble(item2.NetWeight) + " Rate  " + Conversion.ToDouble(item2.Rate)));
						VoucherDetail vd1 = new VoucherDetail();
						vd1.LineId = LineId;
						vd1.AccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd1.AgainstAccountId = Conversion.ToInt(find2[0].PurchaseGLAC);
						vd1.Comments = Conversion.ToString(Remarks);
						vd1.CreditAmount = Conversion.ToDouble(item2.Amount);
						vd1.QtyOut = Conversion.ToDouble(item2.ItemQty);
						vd1.WeightOut = Conversion.ToDouble(item2.NetWeight);
						vd1.ItemRate = Conversion.ToDouble(item2.Rate);
						vd1.ItemAmount = Conversion.ToDouble(item2.Amount);
						voucherHead.voucherDetailList.Add(vd1);
						voucheramt += vd1.CreditAmount;
						LineId++;
					}
				}
			}
			foreach (ConversionOverHeadsDetail item4 in obj.ConversionOverHeadsDetailList.ToList())
			{
				if (item4.OhRemarks == string.Empty)
				{
					item4.OhRemarks = "OverHeads ";
					if (item4.Qty > 0m)
					{
						item4.OhRemarks = item4.OhRemarks + "Qty " + item4.Qty;
					}
					if (item4.Rate > 0m)
					{
						item4.OhRemarks = item4.OhRemarks + " Rate " + Conversion.ToDouble(item4.Rate);
					}
					if (item4.Amount > 0m)
					{
						item4.OhRemarks = item4.OhRemarks + " Amount " + Conversion.ToDouble(item4.Amount);
					}
				}
				VoucherDetail vd4 = new VoucherDetail();
				vd4.AccountId = Conversion.ToInt(item4.OverHeadAccountId);
				vd4.AgainstAccountId = Conversion.ToInt(item4.OverHeadAccountId);
				vd4.Comments = Conversion.ToString(item4.OhRemarks);
				vd4.CreditAmount = Conversion.ToDouble(item4.Amount);
				vd4.QtyOut = Conversion.ToDouble(item4.Qty);
				vd4.WeightOut = Conversion.ToDouble(item4.Qty);
				vd4.ItemRate = Conversion.ToDouble(item4.Rate);
				vd4.ItemAmount = Conversion.ToDouble(item4.Amount);
				voucherHead.voucherDetailList.Add(vd4);
			}
			if (obj.ConversionWagesDetailList.Count > 0)
			{
				dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				foreach (ConversionWagesDetail item in obj.ConversionWagesDetailList.ToList())
				{
					if (item.Remarks == string.Empty)
					{
						item.Remarks = "Wages ";
						if (item.Qty > 0m)
						{
							ConversionWagesDetail conversionWagesDetail = item;
							conversionWagesDetail.Remarks = conversionWagesDetail.Remarks + "Qty " + item.Qty;
						}
						if (item.Rate > 0m)
						{
							ConversionWagesDetail conversionWagesDetail2 = item;
							conversionWagesDetail2.Remarks = conversionWagesDetail2.Remarks + " Rate " + item.Rate;
						}
						if (item.Amount > 0m)
						{
							ConversionWagesDetail conversionWagesDetail3 = item;
							conversionWagesDetail3.Remarks = conversionWagesDetail3.Remarks + " Amount " + item.Amount;
						}
					}
					VoucherDetail vd3 = new VoucherDetail();
					List<SupplierCustomerGlAccountId> find = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.ContractorId)).ToList();
					vd3.AccountId = Conversion.ToInt(find[0].GlAccountId);
					if (find.Count > 0)
					{
						vd3.AgainstAccountId = Conversion.ToInt(find[0].GlAccountId);
						vd3.Comments = Conversion.ToString(item.Remarks);
						vd3.CreditAmount = Conversion.ToDouble(item.Amount);
						vd3.QtyIn = Conversion.ToDouble(item.Qty);
						vd3.WeightIn = Conversion.ToDouble(item.Qty);
						vd3.ItemRate = Conversion.ToDouble(item.Rate);
						vd3.ItemAmount = Conversion.ToDouble(item.Amount);
						vd3.SubsidiaryTypeId = 1;
						vd3.SubsidiaryAccountId = Conversion.ToInt(item.ContractorId);
						vd3.SupplierCustomerId = Conversion.ToInt(item.ContractorId);
						voucherHead.voucherDetailList.Add(vd3);
						continue;
					}
					throw new Exception("Contractor GlAccountId Not Found");
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		voucherHead.VoucherAmount = voucheramt;
		return voucherHead;
	}

	public static int Save(Architecture.Model.pcc.Production.ConversionHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				obj.ModifyUserId = 0;
				return new Architecture.DAL.pcc.Production.ConversionHeader().SetData(obj, "[pcc].[USP_ConversionHeader_Insert]");
			}
			obj.ActionId = 2;
			obj.EntryUserId = 0;
			return new Architecture.DAL.pcc.Production.ConversionHeader().SetData(obj, "[pcc].[USP_ConversionHeader_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.Production.ConversionHeader ReadById(int Id)
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
			new List<Architecture.Model.pcc.Production.ConversionHeader>();
			return new Architecture.DAL.pcc.Production.ConversionHeader().GetData("[pcc].[USP_ConversionHeader_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromConversionHeader(ReportsParameters obj)
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[pcc].[USP_GetDataForDropDownFromConversionHeader]", p);
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.Production.ConversionHeader>("[pcc].[USP_ConversionHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingCoversionsForApproval(ReportsParameters obj)
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
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ConversionNo",
					Value = obj.Id
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.Production.ConversionHeader>("pcc.USP_PendingConversionsForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.pcc.Production.ConversionHeader obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.pcc.Production.ConversionHeader>("[pcc].[USP_ConversionHeader_GetAllMethod]", p);
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
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.Production.ConversionHeader>("[pcc].[USP_ConversionHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Conversion_SlipAndRegister(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.Production.ConversionHeader>("[pcc].[USP_Conversion_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet Conversion_Report(ReportsParameters obj)
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
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
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
			return GenericProvider.GetDataSetProc<Architecture.Model.pcc.Production.ConversionHeader>("[pcc].[USP_Conversion_Report]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ConversionAllDetails_SubReport(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ConversionHeaderId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.Production.ConversionHeader>("[pcc].[USP_ConversionAllDetails_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
