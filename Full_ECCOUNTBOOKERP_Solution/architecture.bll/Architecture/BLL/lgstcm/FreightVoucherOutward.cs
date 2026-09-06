using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.lgstcm;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.lgstcm;

namespace Architecture.BLL.lgstcm;

public class FreightVoucherOutward
{
	public static VoucherHead MakeVoucher(Architecture.Model.lgstcm.FreightVoucherOutward obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		VoucherHead voucher = new VoucherHead();
		try
		{
			voucher.DocumentTypeId = obj.documentTypeId;
			voucher.DocumentTypeSrNo = obj.FreightVoucherOutwardId;
			voucher.RefDocNoId = obj.FreightVoucherOutwardId;
			voucher.VoucherCode = Conversion.ToInt(obj.DocNo);
			voucher.VoucherDate = obj.DocDate;
			voucher.BranchId = obj.BranchesId;
			voucher.ProjectId = obj.ProjectsId;
			voucher.OrganizationId = obj.OrganizationId;
			voucher.CompanyId = obj.CompanyId;
			voucher.FinancialYearId = obj.FinancialYearId;
			voucher.EntryUser = obj.EntryUserId;
			voucher.ModifyUser = obj.ModifyUserId;
			voucher.EntryDate = DateTime.Now;
			voucher.ModifyDate = DateTime.Now;
			voucher.IncludeWHT = false;
			voucher.Remarks = Conversion.ToString(obj.RemarksHeader);
			voucher.RemarksOtherLingo = string.Empty;
			voucher.ChequeDate = DateTime.Today;
			voucher.voucherDetailList = new List<VoucherDetail>();
			if (obj.chargeToDrAccountId == 0)
			{
				throw new Exception("Charge To Debit Account field is required.");
			}
			voucher.RefAccountId = obj.chargeToDrAccountId;
			int LineId = 1;
			if (obj != null && obj.FreightVoucherOutwardPaymentDetailList?.Count > 0)
			{
				foreach (FreightVoucherOutwardPaymentDetail item in obj.FreightVoucherOutwardPaymentDetailList)
				{
					if (item.ActionTypeId != 3 && item.CreditAmount > 0m)
					{
						int creditAccountId = item.AccountId;
						string VoucherComments2 = "";
						string baseComment2 = string.Join(" ", new string[16]
						{
							(obj.GpSrNo == 0) ? null : $"Gp No: {obj.GpSrNo}",
							(obj.DeliveryOrderNo == 0) ? null : $"Delivery Order No: {obj.DeliveryOrderNo}",
							string.IsNullOrWhiteSpace(obj.vehicleNo) ? null : ("Vehicle No: " + obj.vehicleNo),
							string.IsNullOrWhiteSpace(obj.biltyNo) ? null : ("Bilty No: " + obj.biltyNo),
							(obj.NoOfBags == 0m) ? null : $"NoOfBags: {obj.NoOfBags}",
							(obj.netWeight == 0m) ? null : $"Weight: {obj.netWeight}",
							(obj.freightRate == 0m) ? null : $"Freight Rate: {obj.freightRate}",
							string.IsNullOrWhiteSpace(obj.RateUomCode) ? null : ("Rate Uom: " + obj.RateUomCode),
							(obj.biltyFreight == 0m) ? null : $"Bilty Freight: {obj.biltyFreight}",
							(obj.otherCharges == 0m) ? null : $"Add Other Charges: {obj.otherCharges}",
							(obj.totalbiltyfreight == 0m) ? null : $"Total Bilty Freight: {obj.totalbiltyfreight}",
							(obj.AdvanceOrCashFreight == 0m) ? null : $"Less Advance / Cash: {obj.AdvanceOrCashFreight}",
							(obj.totalFreight == 0m) ? null : $"Net Credit To Transporter: {obj.totalFreight}",
							(item.InstrumentTypeId == 1 && !string.IsNullOrWhiteSpace(item.CheqNo)) ? ("CheqNo: " + item.CheqNo) : null,
							(item.InstrumentTypeId == 1) ? $"CheqDate: {item.CheqDate}" : null,
							(item.InstrumentTypeId != 1 && string.IsNullOrWhiteSpace(item.PayeeTitle)) ? null : ("PayeeTitle: " + item.PayeeTitle)
						}.Where((string x) => !string.IsNullOrWhiteSpace(x)));
						VoucherComments2 = (string.IsNullOrWhiteSpace(item.Remarks) ? baseComment2 : (item.Remarks + " " + baseComment2));
						AddVoucherDetail(voucher, voucher.RefAccountId, creditAccountId, item, isDebit: true, VoucherComments2, LineId);
						AddVoucherDetail(voucher, creditAccountId, voucher.RefAccountId, item, isDebit: false, VoucherComments2, LineId);
					}
					LineId++;
				}
			}
			if (obj.totalFreight > 0m)
			{
				string VoucherComments = "";
				string baseComment = string.Join(" ", new string[13]
				{
					(obj.GpSrNo == 0) ? null : $"Gp No: {obj.GpSrNo}",
					(obj.DeliveryOrderNo == 0) ? null : $"Delivery Order No: {obj.DeliveryOrderNo}",
					string.IsNullOrWhiteSpace(obj.vehicleNo) ? null : ("Vehicle No: " + obj.vehicleNo),
					string.IsNullOrWhiteSpace(obj.biltyNo) ? null : ("Bilty No: " + obj.biltyNo),
					(obj.NoOfBags == 0m) ? null : $"NoOfBags: {obj.NoOfBags}",
					(obj.netWeight == 0m) ? null : $"Weight: {obj.netWeight}",
					(obj.freightRate == 0m) ? null : $"Freight Rate: {obj.freightRate}",
					string.IsNullOrWhiteSpace(obj.RateUomCode) ? null : ("Rate Uom: " + obj.RateUomCode),
					(obj.biltyFreight == 0m) ? null : $"Bilty Freight: {obj.biltyFreight}",
					(obj.otherCharges == 0m) ? null : $"Add Other Charges: {obj.otherCharges}",
					(obj.totalbiltyfreight == 0m) ? null : $"Total Bilty Freight: {obj.totalbiltyfreight}",
					(obj.AdvanceOrCashFreight == 0m) ? null : $"Less Advance / Cash: {obj.AdvanceOrCashFreight}",
					(obj.totalFreight == 0m) ? null : $"Net Credit To Transporter: {obj.totalFreight}"
				}.Where((string x) => !string.IsNullOrWhiteSpace(x)));
				VoucherComments = (string.IsNullOrWhiteSpace(obj.RemarksHeader) ? baseComment : (obj.RemarksHeader + " " + baseComment));
				VoucherDetail detail = new VoucherDetail
				{
					AccountId = voucher.RefAccountId,
					AgainstAccountId = obj.TransporterSupCustId,
					Comments = (string.IsNullOrWhiteSpace(VoucherComments) ? Conversion.ToString(obj.RemarksHeader) : VoucherComments),
					DebitAmount = Conversion.ToDouble(obj.totalFreight),
					CreditAmount = 0.0,
					LineId = LineId
				};
				voucher.voucherDetailList.Add(detail);
				VoucherDetail detail2 = new VoucherDetail
				{
					AccountId = obj.TransporterSupCustId,
					AgainstAccountId = voucher.RefAccountId,
					Comments = (string.IsNullOrWhiteSpace(VoucherComments) ? Conversion.ToString(obj.RemarksHeader) : VoucherComments),
					CreditAmount = Conversion.ToDouble(obj.totalFreight),
					DebitAmount = 0.0,
					LineId = LineId
				};
				voucher.voucherDetailList.Add(detail2);
			}
			voucher.VoucherAmount = Conversion.ToDouble(obj.totalbiltyfreight);
			voucher.BillAmount = Conversion.ToDouble(obj.totalbiltyfreight);
			return voucher;
		}
		catch
		{
			throw;
		}
	}

	private static void AddVoucherDetail(VoucherHead voucher, int accountId, int againstAccountId, FreightVoucherOutwardPaymentDetail item, bool isDebit, string AutoRemarks, int LineId)
	{
		decimal amount = ((item.DebitAmount > 0m) ? item.DebitAmount : item.CreditAmount);
		VoucherDetail detail = new VoucherDetail
		{
			AccountId = accountId,
			AgainstAccountId = againstAccountId,
			Comments = (string.IsNullOrWhiteSpace(AutoRemarks) ? Conversion.ToString(item.Remarks) : AutoRemarks),
			DebitAmount = (isDebit ? Conversion.ToDouble(amount) : 0.0),
			CreditAmount = (isDebit ? 0.0 : Conversion.ToDouble(amount)),
			InstrumentTypeId = Conversion.ToInt(item.InstrumentTypeId),
			InvoiceNoRefId = Conversion.ToInt(item.CheqId),
			CheqNoDetail = Conversion.ToString(item.CheqNo),
			DCheqDate = Conversion.ToDateTime(item.CheqDate),
			PayeeTitle = Conversion.ToString(item.PayeeTitle),
			LineId = LineId
		};
		voucher.voucherDetailList.Add(detail);
	}

	public static int Save(Architecture.Model.lgstcm.FreightVoucherOutward obj)
	{
		try
		{
			obj.ActionId = ((obj.FreightVoucherOutwardId == 0) ? 1 : 2);
			obj.VoucherHeadList = MakeVoucher(obj);
			return new Architecture.DAL.lgstcm.FreightVoucherOutward().SetData(obj, "[lgstcm].[USP_FreightVoucherOutward_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveMulti(Architecture.Model.lgstcm.FreightVoucherOutward obj)
	{
		int returnId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.lgstcm.FreightVoucherOutward item in obj.FreightVoucherOutwardList)
			{
				item.VoucherHeadList = MakeVoucher(item);
				returnId = new Architecture.DAL.lgstcm.FreightVoucherOutward().SetData(item, "[lgstcm].[USP_FreightVoucherOutward_InsertAndUpdate]", sqlTrn, sqlcon);
			}
			sqlTrn.Commit();
			return returnId;
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static Architecture.Model.lgstcm.FreightVoucherOutward ReadById(int Id)
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
			new List<Architecture.Model.lgstcm.FreightVoucherOutward>();
			return new Architecture.DAL.lgstcm.FreightVoucherOutward().GetData("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.lgstcm.FreightVoucherOutward> ReadByMasterId(int MasterId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@MasterId",
				Value = MasterId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.lgstcm.FreightVoucherOutward>();
			return new Architecture.DAL.lgstcm.FreightVoucherOutward().GetData("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.lgstcm.FreightVoucherOutward obj)
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
				Value = obj.documentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p);
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

	public static DataTable FreightVoucherOutward_GetVoucherHeadIds(Architecture.Model.lgstcm.FreightVoucherOutward obj)
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
				Value = obj.documentTypeId
			});
			if (obj.FreightVoucherOutwardId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.FreightVoucherOutwardId
				});
			}
			if (obj.MasterDocId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MasterId",
					Value = obj.MasterDocId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_GetVoucherHeadIds]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateMasterDocNo(Architecture.Model.lgstcm.FreightVoucherOutward obj)
	{
		DataTable dt = null;
		int MasterDoc = 0;
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
				Value = obj.documentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateMasterDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p);
			if (dt.Rows.Count > 0)
			{
				MasterDoc = Conversion.ToInt(dt.Rows[0]["MasterDocNo"]);
			}
			return MasterDoc;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
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
			sqlCommand.CommandText = "[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
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

	public static void DeleteByMasterDocID(int EntryUserId, int Id)
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
			sqlCommand.CommandText = "[lgstcm].[USP_FreightVoucherOutward_DeleteByMasterDocID]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@MasterDocId", Id);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
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
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
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
			if (obj.TransporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransporterId",
					Value = obj.TransporterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromFreightVoucherOutward(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != string.Empty && obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("[lgstcm].[USP_GetDataForDropDownFromFreightVoucherOutward]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GatePassOutward_PendingForFreightVoucherOutward(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.HeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsMasterDoc",
					Value = 1
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[dbo].[USP_GatePassOutward_PendingForFreightVoucherOutward]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseOrderHeader_PendingForFreightVoucherOutward(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BillToPartyId",
				Value = obj.BillToPartyId
			});
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			if (obj.HeaderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsMasterDoc",
					Value = obj.HeaderId
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[lgstcm].[USP_PurchaseOrderHeader_PendingForFreightVoucherOutward]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FreightVoucherOutward_PendingForLogisticServicesBill(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ExImInvoiceId",
				Value = obj.ExImInvoiceId
			});
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = obj.ActionId
				});
			}
			if (obj.Ids != string.Empty && obj.Ids != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FreightVoucherIds",
					Value = obj.Ids
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("[lgstcm].[USP_FreightVoucherOutward_PendingForLogisticServicesBill]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FreightVoucherOutward_Slip(ReportsParameters obj)
	{
		DataTable dtMain = null;
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
			if (obj.Id > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.HeaderId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@MasterDocId",
					Value = obj.HeaderId
				});
			}
			DataSet ds = GenericProvider.GetDataSetProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_Slip]", p);
			if (ds != null && ds.Tables.Count > 0)
			{
				dtMain = ds.Tables[0];
				CommonServices.ReplaceCompLogoColumn(dtMain, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			}
			return dtMain;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FreightVoucherOutward_DataForApproval(ReportsParameters obj)
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
			if (obj.TypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ServiceTypeId",
					Value = obj.TypeId
				});
			}
			if (obj.InvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExportInvoiceId",
					Value = obj.InvoiceId
				});
			}
			if (obj.BillToPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillToPartyId",
					Value = obj.BillToPartyId
				});
			}
			if (obj.BrokerOrServiceProviderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BrokerOrServiceProviderId",
					Value = obj.BrokerOrServiceProviderId
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_DataForApproval]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateApprovalStatus(ApprovalList obj)
	{
		if (obj == null || obj.AllApprovalLists == null || obj.AllApprovalLists.Count == 0)
		{
			return;
		}
		DataTable dt = new DataTable();
		dt.Columns.Add("OrganizationId", typeof(int));
		dt.Columns.Add("CompanyId", typeof(int));
		dt.Columns.Add("FinancialYearId", typeof(int));
		dt.Columns.Add("DocumentTypeId", typeof(int));
		dt.Columns.Add("ApprovedUserId", typeof(int));
		dt.Columns.Add("ReqType", typeof(string));
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("ApprovalRemarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.OrganizationId, item.CompanyId, item.FinancialYearId, item.DocumentTypeId, item.EntryUser, ((object)item.ReqType) ?? ((object)DBNull.Value), item.Id, ((object)item.Comments) ?? ((object)DBNull.Value));
		}
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		using SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			using (SqlCommand cmd = new SqlCommand("[lgstcm].[USP_FreightVoucherOutward_Approval]", sqlcon, sqlTrn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@ApprovalData", dt).SqlDbType = SqlDbType.Structured;
				cmd.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch
		{
			sqlTrn.Rollback();
			throw;
		}
	}

	public static DataTable FreightVoucherOutward_Report(ReportsParameters obj)
	{
		DataTable dtMain = null;
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.EximInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExportInvoiceId",
					Value = obj.EximInvoiceId
				});
			}
			if (obj.BillToPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillToPartyId",
					Value = obj.BillToPartyId
				});
			}
			if (obj.BrokerOrServiceProviderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BrokerOrServiceProviderId",
					Value = obj.BrokerOrServiceProviderId
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
			if (!string.IsNullOrWhiteSpace(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			DataSet ds = GenericProvider.GetDataSetProc<Architecture.Model.lgstcm.FreightVoucherOutward>("[lgstcm].[USP_FreightVoucherOutward_Report]", p);
			if (ds != null && ds.Tables.Count > 0)
			{
				dtMain = ds.Tables[0];
				CommonServices.ReplaceCompLogoColumn(dtMain, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			}
			return dtMain;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
