using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.TaxProject.Common;
using Architecture.Model;
using Architecture.Model.TaxProject.Accounts;

namespace Architecture.DAL.TaxProject.Accounts;

public class PdcInventory : DALBase
{
	public int SetDate(PdcInventoryHeader obj, string ProcName)
	{
		int VoucherHeadIdGet = 0;
		int VoucherHeadId = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (Architecture.Model.TaxProject.Accounts.PdcInventory item4 in obj.PdcInventoriesList)
			{
				item4.PdcInventoryHeaderId = obj.Id;
				item4.EntryDate = obj.EntryDate;
				item4.EntryUser = obj.EntryUser;
				item4.ModifyDate = obj.EntryDate;
				item4.ModifyUser = obj.ModifyUser;
				item4.OrganizationId = obj.OrganizationId;
				item4.CompanyId = obj.CompanyId;
				item4.BranchId = obj.BranchId;
				item4.ProjectId = obj.ProjectId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "SP_PdcInventory_Insert"));
			}
			if (returnID > 0)
			{
				double CheqAmount = 0.0;
				Architecture.Model.TaxProject.Accounts.VoucherHead voucherHead = new Architecture.Model.TaxProject.Accounts.VoucherHead();
				DataTable dtvh = Architecture.DAL.TaxProject.Common.CommonServices.GetVoucherHeadId(obj.OrganizationId, obj.CompanyId, obj.DocumentTypeId, obj.Id);
				if (dtvh.Rows.Count > 0)
				{
					VoucherHeadIdGet = (voucherHead.Id = Conversion.ToInt(dtvh.Rows[0]["Id"]));
				}
				voucherHead.DocumentTypeSrNo = returnID;
				voucherHead.VoucherDate = obj.DocDate;
				voucherHead.VoucherCode = obj.DocNo;
				voucherHead.DocumentTypeId = obj.DocumentTypeId;
				voucherHead.RefAccountId = obj.MGLAccountCrId;
				voucherHead.AgainstAccountId = obj.MGLAccountDrId;
				voucherHead.CheqId = returnID;
				voucherHead.VoucherAmount = Conversion.ToDouble(CheqAmount);
				voucherHead.EntryDate = DateTime.Now;
				voucherHead.ModifyDate = DateTime.Now;
				voucherHead.EntryUser = obj.EntryUser;
				voucherHead.ModifyUser = obj.ModifyUser;
				voucherHead.OrganizationId = obj.OrganizationId;
				voucherHead.CompanyId = obj.CompanyId;
				voucherHead.FinancialYearId = obj.FinancialYearId;
				voucherHead.voucherDetailList = new List<VoucherDetail>();
				int value = 0;
				string list = Architecture.DAL.TaxProject.Common.CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CashInHandAc");
				if (list != string.Empty)
				{
					value = Conversion.ToInt(list);
				}
				string Remarks = "";
				string AccountTitle = "";
				DataTable dt = Architecture.DAL.TaxProject.Common.CommonServices.GLAccountDetailById(obj.OrganizationId, obj.CompanyId, obj.MGLAccountCrId);
				if (dt.Rows.Count > 0)
				{
					AccountTitle = Conversion.ToString(dt.Rows[0]["AccountTitle"]);
				}
				foreach (Architecture.Model.TaxProject.Accounts.PdcInventory item3 in obj.PdcInventoriesList.ToList())
				{
					Remarks = "Cheque # '" + item3.CheqNo + "' Received From '" + AccountTitle + "' To Bank '" + item3.BankName + "' " + item3.Remarks;
					if (value > 0 && item3.BankName == "CASH")
					{
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(obj.MGLAccountDrId);
						vd3.AgainstAccountId = Conversion.ToInt(value);
						vd3.Comments = Remarks;
						vd3.CreditAmount = Conversion.ToDouble(item3.CheqAmount);
						voucherHead.voucherDetailList.Add(vd3);
						VoucherDetail vd4 = new VoucherDetail();
						vd4.AccountId = Conversion.ToInt(value);
						vd4.AgainstAccountId = Conversion.ToInt(obj.MGLAccountDrId);
						vd4.Comments = Remarks;
						vd4.DebitAmount = Conversion.ToDouble(item3.CheqAmount);
						voucherHead.voucherDetailList.Add(vd4);
					}
					VoucherDetail vd = new VoucherDetail();
					vd.AccountId = Conversion.ToInt(obj.MGLAccountCrId);
					vd.AgainstAccountId = Conversion.ToInt(obj.MGLAccountDrId);
					vd.Comments = Remarks;
					vd.CheqNoDetail = Conversion.ToString(item3.CheqNo);
					vd.DCheqDate = Conversion.ToDateTime(item3.CheqDate);
					vd.CreditAmount = Conversion.ToDouble(item3.CheqAmount);
					voucherHead.voucherDetailList.Add(vd);
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = Conversion.ToInt(obj.MGLAccountDrId);
					vd2.AgainstAccountId = Conversion.ToInt(obj.MGLAccountCrId);
					vd2.Comments = Remarks;
					vd2.CheqNoDetail = Conversion.ToString(item3.CheqNo);
					vd2.DCheqDate = Conversion.ToDateTime(item3.CheqDate);
					vd2.DebitAmount = Conversion.ToDouble(item3.CheqAmount);
					voucherHead.voucherDetailList.Add(vd2);
					CheqAmount += item3.CheqAmount;
				}
				VoucherHeadId = ((VoucherHeadIdGet != 0) ? Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Update")) : Convert.ToInt32(GenericProvider.SetProc(sqlTrn, voucherHead, "Sp_VoucherHead_Insert")));
				if (VoucherHeadId > 0)
				{
					voucherHead.Id = VoucherHeadId;
				}
				else
				{
					VoucherHeadId = voucherHead.Id;
				}
				foreach (VoucherDetail item2 in voucherHead.voucherDetailList)
				{
					item2.VoucherHeadId = voucherHead.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_VoucherDetail_Insert"));
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.MGLAccountCrId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.ModifyUser;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<PdcInventoryHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<PdcInventoryHeader> lst = new List<PdcInventoryHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<PdcInventoryHeader>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (PdcInventoryHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadByID"));
					item.PdcInventoriesList = GenericProvider.GetAllDetail<Architecture.Model.TaxProject.Accounts.PdcInventory>(sqlcon, "Sp_PdcInventory_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public int PdcCheqStatusUpdate(int Id, string Status)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_PdcInventory_GetAllMethod");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, Id);
			sqlDataBase.AddInParameter(cmd, "CheqStatus", SqlDbType.NVarChar, Status);
			sqlDataBase.AddInParameter(cmd, "Activity", SqlDbType.NVarChar, "CheqStatusUpdate");
			return Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void PdcCheqDeleteByPdcInventoryId(PdcInventoryHeader obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_PdcInventory_GetAllMethod");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, obj.Id);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.BigInt, obj.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, obj.CompanyId);
			sqlDataBase.AddInParameter(cmd, "Activity", SqlDbType.NVarChar, "DeleteByPdcInventoryId");
			Conversion.ToInt(sqlDataBase.ExecuteNonQuery(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
