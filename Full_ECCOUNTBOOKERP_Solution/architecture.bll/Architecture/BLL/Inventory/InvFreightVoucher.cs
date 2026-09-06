using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvFreightVoucher
{
	public static VoucherHead MakeVoucher(Architecture.Model.Inventory.InvFreightVoucher obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		VoucherHead vh = new VoucherHead
		{
			DocumentTypeId = obj.DocumentTypeId,
			DocumentTypeSrNo = obj.Id,
			RefDocNoId = obj.Id,
			RefAccountId = Conversion.ToInt(obj.CashAccountId),
			VoucherCode = Conversion.ToInt(obj.DocumentNo),
			VoucherDate = obj.DocDate,
			Remarks = Conversion.ToString(obj.RemarksHeader),
			RemarksOtherLingo = string.Empty,
			ChequeDate = DateTime.Today,
			IncludeWHT = false,
			BranchId = 0,
			ProjectId = 0,
			ManualBillNo = string.Empty,
			DueDate = DateTime.Now,
			OrganizationId = obj.OrganizationId,
			CompanyId = obj.CompanyId,
			FinancialYearId = obj.FinancialYearId,
			EntryUser = obj.EntryUserId,
			EntryDate = DateTime.Now,
			ModifyUser = obj.ModifyUserId,
			ModifyDate = DateTime.Now,
			voucherDetailList = new List<VoucherDetail>()
		};
		if (obj.InvFreightVoucherDetailslist == null || obj.InvFreightVoucherDetailslist.Count == 0)
		{
			return vh;
		}
		double totalAmount = 0.0;
		int debitAccountId = 0;
		string remarks = string.Empty;
		int paymentCount = obj.InvFreightVoucherPaymentBreakupList?.Count ?? 0;
		foreach (InvFreightVoucherDetail item2 in obj.InvFreightVoucherDetailslist)
		{
			debitAccountId = Conversion.ToInt(item2.DebitAccountId);
			remarks = Conversion.ToString(item2.RemarksDetail);
			double payableAmount = Conversion.ToDouble(item2.TotalPayableAmount);
			vh.voucherDetailList.Add(new VoucherDetail
			{
				AccountId = debitAccountId,
				AgainstAccountId = Conversion.ToInt(obj.CashAccountId),
				Comments = remarks,
				DebitAmount = payableAmount,
				RateCutAmount = Conversion.ToDouble(item2.LessAny),
				ItemAmount = Conversion.ToDouble(item2.BiltyFreight)
			});
			if (paymentCount == 0)
			{
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = Conversion.ToInt(obj.CashAccountId),
					AgainstAccountId = debitAccountId,
					Comments = remarks,
					CreditAmount = payableAmount,
					RateCutAmount = Conversion.ToDouble(item2.LessAny),
					ItemAmount = Conversion.ToDouble(item2.BiltyFreight)
				});
			}
			totalAmount += payableAmount;
		}
		if (paymentCount > 0)
		{
			foreach (InvFreightVoucherPaymentBreakup item in obj.InvFreightVoucherPaymentBreakupList)
			{
				vh.voucherDetailList.Add(new VoucherDetail
				{
					AccountId = Conversion.ToInt(item.AccountId),
					AgainstAccountId = debitAccountId,
					Comments = (string.IsNullOrWhiteSpace(item.Remarks) ? remarks : Conversion.ToString(item.Remarks)),
					CreditAmount = Conversion.ToDouble(item.Amount),
					InstrumentTypeId = Conversion.ToInt(item.InstrumentTypeId),
					InvoiceNoRefId = Conversion.ToInt(item.CheqId),
					CheqNoDetail = Conversion.ToString(item.CheqNo),
					DCheqDate = Conversion.ToDateTime(item.CheqDate),
					PayeeTitle = Conversion.ToString(item.PayeeTitle)
				});
			}
		}
		vh.AgainstAccountId = debitAccountId;
		vh.VoucherAmount = totalAmount;
		vh.BillAmount = totalAmount;
		return vh;
	}

	public static int Save(Architecture.Model.Inventory.InvFreightVoucher obj)
	{
		try
		{
			if ((obj.InvFreightVoucherDetailslist?.Sum((InvFreightVoucherDetail x) => x.TotalPayableAmount) ?? 0m) > 0m)
			{
				obj.VoucherHeadInvoices = MakeVoucher(obj);
			}
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvFreightVoucher().SetData(obj, "Sp_InvFreightVoucher_Insert");
			}
			return new Architecture.DAL.Inventory.InvFreightVoucher().SetData(obj, "Sp_InvFreightVoucher_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvFreightVoucher GetByID(int Id)
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
			new List<Architecture.Model.Inventory.InvFreightVoucher>();
			return new Architecture.DAL.Inventory.InvFreightVoucher().GetDate("Sp_InvFreightVoucher_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Inventory.InvFreightVoucher obj)
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
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvFreightVoucher>("Sp_InvFreightVoucher_GetAllMethod", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DocumentNo"]);
			}
			return Code;
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("Sp_InvFreightVoucher_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromFreightVoucher(int OrganizationId, int CompanyId, string Activity = null)
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
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[dbo].[USP_GetDataForDropDownFromFreightVoucher]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet FreightVoucherSlipAndRegister(ReportsParameters obj)
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
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoF",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoT",
					Value = obj.GpSrNoT
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.CreditAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CreditAccountId",
					Value = obj.CreditAccountId
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
			if (obj.VehicleNo != null && obj.VehicleNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
				});
			}
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.FreightDiscountAmount != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FreightDiscountAmount",
					Value = obj.FreightDiscountAmount
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
			return GenericProvider.GetDataSetProc<Architecture.Model.Inventory.InvGrn>("SP_FreightVoucherSlipAndRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FreightVoucher_PendingForApproval(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
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
			if (!Conversion.CheckDateTimeNull(obj.GpDateF))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDateGp",
					Value = obj.GpDateF
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.GpDateT))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDateGp",
					Value = obj.GpDateT
				});
			}
			if (obj.GpSrNoF != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoF",
					Value = obj.GpSrNoF
				});
			}
			if (obj.GpSrNoT != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@GpSrNoT",
					Value = obj.GpSrNoT
				});
			}
			if (obj.CreditAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CreditAccountId",
					Value = obj.CreditAccountId
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
			if (obj.CityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CityId",
					Value = obj.CityId
				});
			}
			if (obj.VehicleNo != null && obj.VehicleNo != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("USP_FreightVoucher_PendingForApproval", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void FreightVoucher_UpdateApprovalStatus(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand sqlCommandInv = new SqlCommand("[dbo].[USP_FreightVoucher_UpdateApprovalStatus]", sqlcon, sqlTrn))
			{
				sqlCommandInv.CommandType = CommandType.StoredProcedure;
				sqlCommandInv.Parameters.Add("@OrganizationId", SqlDbType.Int);
				sqlCommandInv.Parameters.Add("@CompanyId", SqlDbType.Int);
				sqlCommandInv.Parameters.Add("@FinancialYearId", SqlDbType.Int);
				sqlCommandInv.Parameters.Add("@DocumentTypeId", SqlDbType.Int);
				sqlCommandInv.Parameters.Add("@ApprovedUserId", SqlDbType.Int);
				sqlCommandInv.Parameters.Add("@ReqType", SqlDbType.VarChar, 150);
				sqlCommandInv.Parameters.Add("@Id", SqlDbType.Int);
				sqlCommandInv.Parameters.Add("@ApprovalRemarks", SqlDbType.VarChar, 5000);
				foreach (ApprovalList item in obj.AllApprovalLists)
				{
					sqlCommandInv.Parameters["@OrganizationId"].Value = item.OrganizationId;
					sqlCommandInv.Parameters["@CompanyId"].Value = item.CompanyId;
					sqlCommandInv.Parameters["@FinancialYearId"].Value = item.FinancialYearId;
					sqlCommandInv.Parameters["@DocumentTypeId"].Value = item.DocumentTypeId;
					sqlCommandInv.Parameters["@ApprovedUserId"].Value = item.EntryUser;
					sqlCommandInv.Parameters["@ReqType"].Value = ((object)item.ReqType) ?? ((object)DBNull.Value);
					sqlCommandInv.Parameters["@Id"].Value = item.Id;
					sqlCommandInv.Parameters["@ApprovalRemarks"].Value = ((object)item.Comments) ?? ((object)DBNull.Value);
					sqlCommandInv.ExecuteNonQuery();
				}
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}
}
