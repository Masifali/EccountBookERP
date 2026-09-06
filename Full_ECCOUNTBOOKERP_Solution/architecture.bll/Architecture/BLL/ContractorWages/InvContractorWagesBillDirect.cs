using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.ContractorWages;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;

namespace Architecture.BLL.ContractorWages;

public class InvContractorWagesBillDirect
{
	public static VoucherHead MakeVoucher(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> dtitem = new List<Architecture.Model.ContractorWages.InvConractorWagesAccounts>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
			int FinancialYearId = 0;
			DataTable dt = CommonServies.GetActiveFinancialYear(obj.OrganizationId, obj.CompanyId);
			if (dt.Rows.Count <= 0)
			{
				throw new Exception("Financial Year Not Found Please Check");
			}
			FinancialYearId = Conversion.ToInt(dt.Rows[0]["Id"]);
			vh.DocumentTypeId = obj.DocumentTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = Conversion.ToString(obj.OtherRemarks);
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectsId;
			vh.BillAmount = 0.0;
			vh.ManualBillNo = Conversion.ToString(obj.RefDocNo);
			vh.DueDate = obj.DocDate;
			vh.DueDays = 0;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProduct")))
			{
				if (obj.DocumentTypeId == 101 && obj.invContractWagesBillDateil.Count > 0)
				{
					foreach (InvContractorWagesBillDetail item2 in obj.invContractWagesBillDateil.ToList())
					{
						List<SupplierCustomerGlAccountId> findSupp = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item2.ContractorId)).ToList();
						if (Conversion.ToInt(item2.PurchaseGLAC) != 0)
						{
							if (findSupp.Count > 0)
							{
								VoucherDetail vd = new VoucherDetail();
								vd.AccountId = Conversion.ToInt(item2.PurchaseGLAC);
								vd.AgainstAccountId = Conversion.ToInt(findSupp[0].GlAccountId);
								vd.Comments = item2.WagesAccountName + " Item Name :" + item2.ItemName + "  Wages Qty :  " + item2.Qty + "   Wages Rate :  " + item2.WageRate + "  Wages Amount :  " + item2.WagesAmount + "  BillWeight :  " + item2.BillWeight;
								vd.DebitAmount = Conversion.ToDouble(item2.WagesAmount);
								vd.CreditAmount = Conversion.ToDouble(0);
								vd.ItemRate = Conversion.ToDouble(item2.WageRate);
								vd.ItemAmount = Conversion.ToDouble(item2.WagesAmount);
								vh.voucherDetailList.Add(vd);
								VoucherDetail vd3 = new VoucherDetail();
								vd3.AccountId = Conversion.ToInt(findSupp[0].GlAccountId);
								vd3.AgainstAccountId = Conversion.ToInt(item2.PurchaseGLAC);
								vd3.Comments = item2.CompanyName + "  Item Name :" + item2.ItemName + "  Wages Qty :  " + item2.Qty + "  Wages Rate :  " + item2.WageRate + "  Wages Amount :  " + item2.WagesAmount + "  BillWeight :  " + item2.BillWeight;
								vd3.DebitAmount = Conversion.ToDouble(0);
								vd3.CreditAmount = Conversion.ToDouble(item2.WagesAmount);
								vd3.ItemRate = Conversion.ToDouble(item2.WageRate);
								vd3.ItemAmount = Conversion.ToDouble(item2.WagesAmount);
								vh.voucherDetailList.Add(vd3);
							}
							continue;
						}
						throw new Exception("PurchaseGLAC Not Found");
					}
				}
			}
			else
			{
				List<SqlParameter> x = new List<SqlParameter>();
				x.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				x.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				x.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "ReadAll"
				});
				dtitem = new Architecture.DAL.ContractorWages.InvConractorWagesAccounts().GetData("Sp_InvConractorWagesAccounts_GetAllMethod", x);
				if (obj.DocumentTypeId == 101 && obj.invContractWagesBillDateil.Count > 0)
				{
					foreach (InvContractorWagesBillDetail item in obj.invContractWagesBillDateil.ToList())
					{
						List<SupplierCustomerGlAccountId> findSupp2 = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.ContractorId)).ToList();
						if (Conversion.ToInt(item.InvConractorWagesAccountsId) != 0 && dtitem.Count > 0)
						{
							List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> find = dtitem.Where((Architecture.Model.ContractorWages.InvConractorWagesAccounts u) => u.Id == Conversion.ToInt(item.InvConractorWagesAccountsId)).ToList();
							if (find.Count <= 0)
							{
								throw new Exception("InvConractorWagesAccount Id Not Found");
							}
							if (findSupp2.Count <= 0)
							{
								throw new Exception("Supplier Customer GlAccountId Not Found");
							}
							VoucherDetail vd2 = new VoucherDetail();
							vd2.AccountId = Conversion.ToInt(find[0].GlAccountId);
							vd2.AgainstAccountId = Conversion.ToInt(findSupp2[0].GlAccountId);
							vd2.Comments = item.WagesAccountName + " Item Name :" + item.ItemName + "  Wages Qty :  " + item.Qty + "  Wages Rate :  " + item.WageRate + "  Wages Amount :  " + item.WagesAmount + "  BillWeight :  " + item.BillWeight;
							vd2.DebitAmount = Conversion.ToDouble(item.WagesAmount);
							vd2.CreditAmount = Conversion.ToDouble(0);
							vd2.ItemRate = Conversion.ToDouble(item.WageRate);
							vd2.ItemAmount = Conversion.ToDouble(item.WagesAmount);
							vh.voucherDetailList.Add(vd2);
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = Conversion.ToInt(findSupp2[0].GlAccountId);
							vd4.AgainstAccountId = Conversion.ToInt(find[0].GlAccountId);
							vd4.Comments = item.CompanyName + "  Item Name :" + item.ItemName + "  Wages Qty :  " + item.Qty + "   Wages Rate :  " + item.WageRate + "  Wages Amount :  " + item.WagesAmount + "  BillWeight :  " + item.BillWeight;
							vd4.DebitAmount = Conversion.ToDouble(0);
							vd4.CreditAmount = Conversion.ToDouble(item.WagesAmount);
							vd4.ItemRate = Conversion.ToDouble(item.WageRate);
							vd4.ItemAmount = Conversion.ToDouble(item.WagesAmount);
							vh.voucherDetailList.Add(vd4);
						}
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

	public static int Save(Architecture.Model.ContractorWages.InvContractorWagesBillDirect obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.ContractorWages.InvContractorWagesBillDirect().SetData(obj, "Sp_InvContractorWagesBillDirect_Insert");
			}
			return new Architecture.DAL.ContractorWages.InvContractorWagesBillDirect().SetData(obj, "Sp_InvContractorWagesBillDirect_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.ContractorWages.InvContractorWagesBillDirect obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillDirect>("Sp_InvContractorWagesBillDirect_GetAllMethod", p);
			if (dt.Rows.Count > 0)
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

	public static Architecture.Model.ContractorWages.InvContractorWagesBillDirect GetByID(int Id)
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
			new List<Architecture.Model.ContractorWages.InvContractorWagesBillDirect>();
			return new Architecture.DAL.ContractorWages.InvContractorWagesBillDirect().GetData("Sp_InvContractorWagesBillDirect_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.ContractorWages.InvContractorWagesBillDirect obj)
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
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillDirect>("Sp_InvContractorWagesBillDirect_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvContractorWagesBillDirect_SlipAndRegister(ReportsParameters obj)
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
			if (obj.Id > 0)
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
			if (obj.FromDocNo > 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo > 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.ContractorId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContractorId",
					Value = obj.ContractorId
				});
			}
			if (obj.WagesItemId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WagesItemId",
					Value = obj.WagesItemId
				});
			}
			if (obj.ItemId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProductItemId",
					Value = obj.ItemId
				});
			}
			if (obj.PackSizeFrom > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackSizeFrom",
					Value = obj.PackSizeFrom
				});
			}
			if (obj.PackSizeTo > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackSizeTo",
					Value = obj.PackSizeTo
				});
			}
			if (obj.RateFrom > 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RateFrom",
					Value = obj.RateFrom
				});
			}
			if (obj.RateTo > 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RateTo",
					Value = obj.RateTo
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("[dbo].[InvContractorWagesBillDirect_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
