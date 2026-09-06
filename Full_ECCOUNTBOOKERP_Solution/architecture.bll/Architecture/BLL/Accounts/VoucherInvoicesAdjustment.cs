using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class VoucherInvoicesAdjustment
{
	public static int Save(Architecture.Model.Accounts.VoucherInvoicesAdjustment obj)
	{
		try
		{
			if (obj.TransactionTypeId == 3)
			{
				Architecture.Model.Export.ExImFCBankReceipts objFcyReceipts = Architecture.BLL.Export.ExImFCBankReceipts.GetByID(VoucherHead.GetVoucherHeaderByID(obj.VoucherHeadId).DocumentTypeSrNo);
				if (objFcyReceipts != null)
				{
					decimal TotalInvoiceAmount = obj.VoucherInvoicesAdjustmentList.Sum((Architecture.Model.Accounts.VoucherInvoicesAdjustment x) => (!(x.AdjustmentAmount > 0m) || !(x.ExchangeRate > 0m)) ? 0m : (x.AdjustmentAmount * x.ExchangeRate));
					objFcyReceipts.FcBankVoucherHead = Architecture.BLL.Export.ExImFCBankReceipts.MakeVoucher(objFcyReceipts, obj, TotalInvoiceAmount);
				}
				return new Architecture.DAL.Accounts.VoucherInvoicesAdjustment().SetData(obj, "USP_VoucherInvoicesAdjustment_InsertUpdateDelete", objFcyReceipts);
			}
			return new Architecture.DAL.Accounts.VoucherInvoicesAdjustment().SetData(obj, "USP_VoucherInvoicesAdjustment_InsertUpdateDelete");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByVoucherHeadId(int TransactionTypeId, int VoucherHeadId, int PaymentTypeId, int SupplierCustomerId, int PartyGlId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TransactionTypeId",
				Value = TransactionTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherHeadId",
				Value = VoucherHeadId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PaymentTypeId",
				Value = PaymentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PartyGlId",
				Value = PartyGlId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherInvoicesAdjustment>("[dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId]", p);
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
				ParameterName = "@TransactionTypeId",
				Value = obj.TransactionTypeId
			});
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDateTo",
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.SupplierCustomerId != 0 && obj.GlAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@PartyGlId",
					Value = obj.GlAccountId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherInvoicesAdjustment>("[dbo].[USP_VoucherInvoicesAdjustment_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Vouchers_OutstandingForAdjustment(int OrganizationId, int CompanyId, int TransactionTypeId, int AccountId = 0)
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
				ParameterName = "@TransTypeId",
				Value = TransactionTypeId
			});
			if (AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = AccountId
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_Vouchers_OutstandingForAdjustment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoice_GetDataForAdjustment(int OrganizationId, int CompanyId, int SupplierCustomerId)
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
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_PurchaseInvoice_GetDataForAdjustment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SaleInvoice_GetDataForAdjustment(int OrganizationId, int CompanyId, int SupplierCustomerId)
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
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_SaleInvoice_GetDataForAdjustment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PartyDropDown(int TransactionTypeId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@TransactionTypeId",
				Value = TransactionTypeId
			});
			return GenericProvider.GetDataTableProc<InvGdn>("[dbo].[USP_VoucherInvoicesAdjustment_PartyDropDown]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Vouchers_OutstandingFcyReceiptsForAdjustment(int OrganizationId, int CompanyId, int AccountId = 0, int FcyId = 0)
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
			if (AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = AccountId
				});
			}
			if (FcyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FcyId",
					Value = FcyId
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_Vouchers_OutstandingFcyReceiptsForAdjustment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportInvoice_GetDataForAdjustment(int OrganizationId, int CompanyId, int SupplierCustomerId, int FcyId = 0)
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
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FcyId",
				Value = FcyId
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[dbo].[USP_ExportInvoice_GetDataForAdjustment]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
