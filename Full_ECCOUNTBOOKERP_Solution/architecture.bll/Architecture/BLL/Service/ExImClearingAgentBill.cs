using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Service;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.Service;

namespace Architecture.BLL.Service;

public class ExImClearingAgentBill
{
	public static VoucherHead MakeVoucher(Architecture.Model.Service.ExImClearingAgentBill obj)
	{
		List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
		List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
		new DataTable();
		VoucherHead vh = new VoucherHead();
		try
		{
			vh.DocumentTypeId = obj.DocTypeId;
			vh.DocumentTypeSrNo = obj.Id;
			vh.RefDocNoId = obj.Id;
			vh.VoucherCode = Conversion.ToInt(obj.DocNo);
			vh.VoucherDate = obj.DocDate;
			vh.Remarks = "";
			vh.RemarksOtherLingo = "";
			vh.ChequeDate = DateTime.Today;
			vh.IncludeWHT = false;
			vh.BranchId = obj.BranchesId;
			vh.ProjectId = obj.ProjectId;
			vh.ManualBillNo = Conversion.ToString(obj.RefBillNo);
			vh.DueDate = obj.DueDate;
			vh.OrganizationId = obj.OrganizationId;
			vh.CompanyId = obj.CompanyId;
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtitem = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			if (dtSupplier.Count <= 0)
			{
				throw new Exception("PartyName GlAccountId nof found");
			}
			List<SupplierCustomerGlAccountId> Supplier = dtSupplier.Where((SupplierCustomerGlAccountId x) => x.Id == Conversion.ToInt(obj.SupCustId)).ToList();
			if (Supplier.Count <= 0)
			{
				throw new Exception("PartyName GlAccountId nof found");
			}
			vh.RefAccountId = Conversion.ToInt(Supplier[0].GlAccountId);
			if (obj.BillDetail.Count > 0)
			{
				double TotalAmount = 0.0;
				foreach (ExImClearingAgentBillDetail item in obj.BillDetail.ToList())
				{
					if (item.DebitAccountId > 0)
					{
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(item.DebitAccountId);
						vd4.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
						vd4.Comments = Conversion.ToString(item.Description);
						vd4.DebitAmount = Conversion.ToDouble(item.TotalAmount);
						vd4.ItemId = Conversion.ToInt(item.ItemId);
						vd4.DMultiCurrencyId = Conversion.ToInt(item.CurrencyId);
						vd4.DExchangeCurrencyRate = Conversion.ToDouble(item.ExhangeRate);
						vd4.ItemRate = Conversion.ToDouble(item.ItemRate);
						vd4.ItemAmount = Conversion.ToDouble(item.TotalAmount);
						vh.voucherDetailList.Add(vd4);
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = Conversion.ToInt(vh.RefAccountId);
						vd2.AgainstAccountId = Conversion.ToInt(item.DebitAccountId);
						vd2.Comments = Conversion.ToString(item.Description);
						vd2.CreditAmount = Conversion.ToDouble(item.TotalAmount);
						vd2.ItemId = Conversion.ToInt(item.ItemId);
						vd2.DMultiCurrencyId = Conversion.ToInt(item.CurrencyId);
						vd2.DExchangeCurrencyRate = Conversion.ToDouble(item.ExhangeRate);
						vd2.ItemRate = Conversion.ToDouble(item.ItemRate);
						vd2.ItemAmount = Conversion.ToDouble(item.TotalAmount);
						vh.voucherDetailList.Add(vd2);
						TotalAmount += item.TotalAmount;
						continue;
					}
					if (dtitem.Count > 0)
					{
						List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(item.ItemId)).ToList();
						if (find.Count > 0)
						{
							VoucherDetail vd5 = new VoucherDetail();
							vd5.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd5.AgainstAccountId = Conversion.ToInt(vh.RefAccountId);
							vd5.Comments = Conversion.ToString(item.Description);
							vd5.DebitAmount = Conversion.ToDouble(item.TotalAmount);
							vd5.ItemId = Conversion.ToInt(item.ItemId);
							vd5.DMultiCurrencyId = Conversion.ToInt(item.CurrencyId);
							vd5.DExchangeCurrencyRate = Conversion.ToDouble(item.ExhangeRate);
							vd5.ItemRate = Conversion.ToDouble(item.ItemRate);
							vd5.ItemAmount = Conversion.ToDouble(item.TotalAmount);
							vh.voucherDetailList.Add(vd5);
							VoucherDetail vd3 = new VoucherDetail();
							vd3.AccountId = Conversion.ToInt(vh.RefAccountId);
							vd3.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd3.Comments = Conversion.ToString(item.Description);
							vd3.CreditAmount = Conversion.ToDouble(item.TotalAmount);
							vd3.ItemId = Conversion.ToInt(item.ItemId);
							vd3.DMultiCurrencyId = Conversion.ToInt(item.CurrencyId);
							vd3.DExchangeCurrencyRate = Conversion.ToDouble(item.ExhangeRate);
							vd3.ItemRate = Conversion.ToDouble(item.ItemRate);
							vd3.ItemAmount = Conversion.ToDouble(item.TotalAmount);
							vh.voucherDetailList.Add(vd3);
							TotalAmount += item.TotalAmount;
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

	public static int Save(Architecture.Model.Service.ExImClearingAgentBill obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
				return new Architecture.DAL.Service.ExImClearingAgentBill().SetDate(obj, "Sp_ExImClearingAgentBill_Insert");
			}
			obj.VoucherHeadInvoices = MakeVoucher(obj);
			return new Architecture.DAL.Service.ExImClearingAgentBill().SetDate(obj, "Sp_ExImClearingAgentBill_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Service.ExImClearingAgentBill GetByID(int Id)
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
			new List<Architecture.Model.Service.ExImClearingAgentBill>();
			return new Architecture.DAL.Service.ExImClearingAgentBill().GetDate("Sp_ExImClearingAgentBill_GetAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryData(Architecture.Model.Service.ExImClearingAgentBill obj)
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
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Service.ExImClearingAgentBill>("Sp_ExImClearingAgentBill_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryDirect(ReportsParameters obj)
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
				ParameterName = "@DocTypeId",
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Service.ExImClearingAgentBill>("Sp_ExImClearingAgentBill_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Service.ExImClearingAgentBill obj)
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
				ParameterName = "@DocTypeId",
				Value = obj.DocTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNoCompanyIdOrganizationId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Service.ExImClearingAgentBill>("Sp_ExImClearingAgentBill_GetAll", p);
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

	public static DataTable GetDataForDropDownFromClearingAgentBill(int OrganizationId, int CompanyId, string Activity = null)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Service.ExImClearingAgentBill>("USP_GetDataForDropDownFromClearingAgentBill", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int DeleteUserId, int Id)
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
			sqlCommand.CommandText = "[dbo].[Sp_ExImClearingAgentBill_GetAll]";
			sqlCommand.Parameters.AddWithValue("@DeleteUserId", DeleteUserId);
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
}
