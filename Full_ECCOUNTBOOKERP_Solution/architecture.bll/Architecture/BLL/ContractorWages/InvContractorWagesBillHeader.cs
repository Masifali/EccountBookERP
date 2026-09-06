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
using Architecture.Model.Production;

namespace Architecture.BLL.ContractorWages;

public class InvContractorWagesBillHeader
{
	public static VoucherHead MakeVoucher(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
	{
		VoucherHead vh = new VoucherHead();
		try
		{
			List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> dtitem = new List<Architecture.Model.ContractorWages.InvConractorWagesAccounts>();
			List<SupplierCustomerGlAccountId> dtSupplier = new List<SupplierCustomerGlAccountId>();
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
			vh.FinancialYearId = obj.FinancialYearId;
			vh.EntryUser = obj.EntryUser;
			vh.EntryDate = DateTime.Now;
			vh.ModifyDate = DateTime.Now;
			vh.ModifyUser = obj.ModifyUser;
			vh.voucherDetailList = new List<VoucherDetail>();
			dtSupplier = CommonServies.GetSupplierCustomerListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
			List<GetItemGlIdsandItemName> itemlst = new List<GetItemGlIdsandItemName>();
			bool ProductionWagesChargeToWIPAccount = false;
			bool ContractWagesChargetoProduct = false;
			bool ContractorWagesChargetoContractAccountOnForwarding = false;
			if (obj.DocumentTypeId != 219 && (obj.RefDocumentTypeId == 80 || obj.RefDocumentTypeId == 112))
			{
				ProductionWagesChargeToWIPAccount = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ProductionWagesChargeToWIPAccount"));
			}
			else if (obj.DocumentTypeId == 101 && obj.RefDocumentTypeId == 205)
			{
				ContractorWagesChargetoContractAccountOnForwarding = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "LabourWagesChargedtoContractAccountonForwarding"));
			}
			else if (obj.DocumentTypeId == 101 && (obj.RefDocumentTypeId == 46 || obj.RefDocumentTypeId == 68 || obj.RefDocumentTypeId == 806))
			{
				itemlst = CommonServies.GetItemListForFinancialEffects(obj.OrganizationId, obj.CompanyId);
				ContractWagesChargetoProduct = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "ContractWagesChargetoProduct"));
			}
			if ((obj.DocumentTypeId == 101 || obj.DocumentTypeId == 810) && obj.invContractWagesBillDateil.Count > 0)
			{
				foreach (InvContractorWagesBillDetail item2 in obj.invContractWagesBillDateil.ToList())
				{
					if (item2.FreeOfCost)
					{
						continue;
					}
					int DebitAccountId2 = 0;
					if (ProductionWagesChargeToWIPAccount && (obj.RefDocumentTypeId == 80 || obj.RefDocumentTypeId == 112 || obj.RefDocumentTypeId == 181))
					{
						if (obj.DocumentTypeId == 101)
						{
							List<SqlParameter> p3 = new List<SqlParameter>();
							p3.Add(new SqlParameter
							{
								ParameterName = "@OrganizationId",
								Value = obj.OrganizationId
							});
							p3.Add(new SqlParameter
							{
								ParameterName = "@CompanyId",
								Value = obj.CompanyId
							});
							p3.Add(new SqlParameter
							{
								ParameterName = "@Id",
								Value = obj.RefDocNoId
							});
							p3.Add(new SqlParameter
							{
								ParameterName = "@Activity",
								Value = "GetWipAccountsByProductionId"
							});
							DataTable dtJobOrder2 = GenericProvider.GetDataTableProc<InvProductionJobOrder>("Sp_InvProductionJobOrder_GetAllMethod", p3);
							if (dtJobOrder2 == null || dtJobOrder2.Rows.Count <= 0)
							{
								throw new Exception("Work in process Account not found against JobOrder");
							}
							DebitAccountId2 = Conversion.ToInt(dtJobOrder2.Rows[0]["WIPAccountId"]);
						}
						else
						{
							List<SqlParameter> p2 = new List<SqlParameter>();
							p2.Add(new SqlParameter
							{
								ParameterName = "@OrganizationId",
								Value = obj.OrganizationId
							});
							p2.Add(new SqlParameter
							{
								ParameterName = "@CompanyId",
								Value = obj.CompanyId
							});
							p2.Add(new SqlParameter
							{
								ParameterName = "@Id",
								Value = obj.JobOrderId
							});
							p2.Add(new SqlParameter
							{
								ParameterName = "@Activity",
								Value = "GetGlAccountsByJobOrderId"
							});
							DataTable dtJobOrder = GenericProvider.GetDataTableProc<InvProductionJobOrder>("Sp_InvProductionJobOrder_GetAllMethod", p2);
							if (dtJobOrder == null || dtJobOrder.Rows.Count <= 0)
							{
								throw new Exception("Work in process Account not found against JobOrder");
							}
							DebitAccountId2 = Conversion.ToInt(dtJobOrder.Rows[0]["WorkInProccessAcId"]);
						}
					}
					if (ContractWagesChargetoProduct)
					{
						List<GetItemGlIdsandItemName> find3 = itemlst.Where((GetItemGlIdsandItemName o) => o.Id == Conversion.ToInt(item2.ItemId)).ToList();
						if (find3.Count <= 0)
						{
							throw new Exception("ItemId Not Found");
						}
						DebitAccountId2 = Conversion.ToInt(find3[0].PurchaseGLAC);
					}
					if (ContractorWagesChargetoContractAccountOnForwarding)
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
							ParameterName = "@Id",
							Value = obj.RefDocNoId
						});
						DataTable dt = GenericProvider.GetDataTableProc<InvProductionJobOrder>("usp_getExportInvoiceDataByForwardingId", p);
						if (dt == null || dt.Rows.Count <= 0)
						{
							throw new Exception("Contract Credit Account Not found against Forwarding");
						}
						DebitAccountId2 = Conversion.ToInt(dt.Rows[0]["ContractGLAcId"]);
					}
					if (DebitAccountId2 == 0)
					{
						if (dtitem.Count == 0 || dtitem == null)
						{
							List<SqlParameter> x2 = new List<SqlParameter>();
							x2.Add(new SqlParameter
							{
								ParameterName = "@OrganizationId",
								Value = obj.OrganizationId
							});
							x2.Add(new SqlParameter
							{
								ParameterName = "@CompanyId",
								Value = obj.CompanyId
							});
							x2.Add(new SqlParameter
							{
								ParameterName = "@Activity",
								Value = "ReadAll"
							});
							dtitem = new Architecture.DAL.ContractorWages.InvConractorWagesAccounts().GetData("Sp_InvConractorWagesAccounts_GetAllMethod", x2);
						}
						if (dtitem.Count <= 0)
						{
							throw new Exception("Contactor Wages Account Id Not Found");
						}
						List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> find2 = dtitem.Where((Architecture.Model.ContractorWages.InvConractorWagesAccounts u) => u.Id == Conversion.ToInt(item2.InvConractorWagesAccountsId)).ToList();
						if (find2.Count <= 0)
						{
							throw new Exception("Contactor Wages Account Id Not Found");
						}
						DebitAccountId2 = Conversion.ToInt(find2[0].GlAccountId);
					}
					if (DebitAccountId2 == 0)
					{
						throw new Exception("Debit AccountId Not Found");
					}
					List<SupplierCustomerGlAccountId> findSupp2 = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item2.ContractorId)).ToList();
					if (findSupp2.Count <= 0)
					{
						continue;
					}
					if (obj.DocumentTypeId == 810 && !item2.IsCompany)
					{
						List<SupplierCustomerGlAccountId> findParty2 = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.StockPartyId)).ToList();
						if (findParty2.Count <= 0)
						{
							throw new Exception("StockParty GlAccountId not found against " + obj.StockPartyId);
						}
						DebitAccountId2 = Conversion.ToInt(findParty2[0].GlAccountId);
					}
					string Remarks2 = "";
					VoucherDetail vd2 = new VoucherDetail();
					vd2.AccountId = DebitAccountId2;
					vd2.AgainstAccountId = Conversion.ToInt(findSupp2[0].GlAccountId);
					Remarks2 = (vd2.Comments = obj.RefDocument + "  :" + item2.WagesAccountName + " Item Name :" + item2.ItemName + "  Wages Qty :  " + item2.Qty + "  Wages Rate :  " + item2.WageRate + "  Wages Amount :  " + item2.WagesAmount + "  BillWeight :  " + item2.BillWeight);
					if (!item2.FreeOfCost)
					{
						vd2.DebitAmount = Conversion.ToDouble(item2.WagesAmount);
						vd2.ItemAmount = Conversion.ToDouble(item2.WagesAmount);
					}
					else
					{
						vd2.DebitAmount = 0.0;
						vd2.ItemAmount = 0.0;
					}
					vd2.CreditAmount = Conversion.ToDouble(0);
					vd2.ItemRate = Conversion.ToDouble(item2.WageRate);
					vd2.OrderNo = obj.JobOrderId;
					vh.voucherDetailList.Add(vd2);
					VoucherDetail vd4 = new VoucherDetail();
					vd4.AccountId = Conversion.ToInt(findSupp2[0].GlAccountId);
					vd4.AgainstAccountId = DebitAccountId2;
					vd4.Comments = Remarks2;
					vd4.DebitAmount = Conversion.ToDouble(0);
					if (!item2.FreeOfCost)
					{
						vd4.CreditAmount = Conversion.ToDouble(item2.WagesAmount);
						vd4.ItemAmount = Conversion.ToDouble(item2.WagesAmount);
					}
					else
					{
						vd4.CreditAmount = 0.0;
						vd4.ItemAmount = 0.0;
					}
					vd4.ItemRate = Conversion.ToDouble(item2.WageRate);
					vd4.OrderNo = obj.JobOrderId;
					vh.voucherDetailList.Add(vd4);
				}
			}
			if (obj.DocumentTypeId == 219 && obj.invContractWagesBillDateil.Count > 0)
			{
				if (obj.StockPartyId == 0)
				{
					throw new Exception("StockPartyId cannot be equal to zero");
				}
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
				foreach (InvContractorWagesBillDetail item in obj.invContractWagesBillDateil.ToList())
				{
					int DebitAccountId = 0;
					List<SupplierCustomerGlAccountId> findSupp = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(item.ContractorId)).ToList();
					if (findSupp.Count > 0)
					{
						if (item.IsCompany)
						{
							if (dtitem.Count <= 0)
							{
								throw new Exception("Contactor Wages Accounts data is empty");
							}
							List<Architecture.Model.ContractorWages.InvConractorWagesAccounts> find = dtitem.Where((Architecture.Model.ContractorWages.InvConractorWagesAccounts u) => u.Id == Conversion.ToInt(item.InvConractorWagesAccountsId)).ToList();
							if (find.Count <= 0)
							{
								throw new Exception("Activity GlAccountId not found against " + item.InvConractorWagesAccountsId);
							}
							DebitAccountId = Conversion.ToInt(find[0].GlAccountId);
						}
						else
						{
							List<SupplierCustomerGlAccountId> findParty = dtSupplier.Where((SupplierCustomerGlAccountId c) => c.Id == Conversion.ToInt(obj.StockPartyId)).ToList();
							if (findParty.Count <= 0)
							{
								throw new Exception("StockParty GlAccountId not found against " + obj.StockPartyId);
							}
							DebitAccountId = Conversion.ToInt(findParty[0].GlAccountId);
						}
						if (DebitAccountId == 0)
						{
							throw new Exception("DebitAccountId cannot be equal to zero for Party Processing");
						}
						string Remarks = "";
						VoucherDetail vd = new VoucherDetail();
						vd.AccountId = DebitAccountId;
						vd.AgainstAccountId = Conversion.ToInt(findSupp[0].GlAccountId);
						Remarks = (vd.Comments = obj.RefDocument + "  :" + item.WagesAccountName + " Item Name :" + item.ItemName + "  Wages Qty :  " + item.Qty + "  Wages Rate :  " + item.WageRate + "  Wages Amount :  " + item.WagesAmount + "  BillWeight :  " + item.BillWeight);
						vd.DebitAmount = Conversion.ToDouble(item.WagesAmount);
						vd.ItemAmount = Conversion.ToDouble(item.WagesAmount);
						vd.ItemRate = Conversion.ToDouble(item.WageRate);
						vd.OrderNo = obj.JobOrderId;
						vh.voucherDetailList.Add(vd);
						VoucherDetail vd3 = new VoucherDetail();
						vd3.AccountId = Conversion.ToInt(findSupp[0].GlAccountId);
						vd3.AgainstAccountId = DebitAccountId;
						vd3.Comments = Remarks;
						vd3.CreditAmount = Conversion.ToDouble(item.WagesAmount);
						vd3.ItemAmount = Conversion.ToDouble(item.WagesAmount);
						vd3.ItemRate = Conversion.ToDouble(item.WageRate);
						vd3.OrderNo = obj.JobOrderId;
						vh.voucherDetailList.Add(vd3);
						continue;
					}
					throw new Exception("Contactor GlAccountId not found against " + item.ContractorId);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return vh;
	}

	public static int Save(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				obj.VoucherHeadForContractorWages = MakeVoucher(obj);
				return new Architecture.DAL.ContractorWages.InvContractorWagesBillHeader().SetData(obj, "Sp_InvContractorWagesBillHeader_Insert");
			}
			obj.VoucherHeadForContractorWages = MakeVoucher(obj);
			return new Architecture.DAL.ContractorWages.InvContractorWagesBillHeader().SetData(obj, "Sp_InvContractorWagesBillHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
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
			dt = GenericProvider.GetDataTableProc<SaleOrder>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
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

	public static Architecture.Model.ContractorWages.InvContractorWagesBillHeader GetByID(int Id)
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
			new List<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>();
			return new Architecture.DAL.ContractorWages.InvContractorWagesBillHeader().GetData("Sp_InvContractorWagesBillHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetContractorWagesBillHeaderIdByDocNo(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
	{
		DataTable dt = null;
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdbyDocNo"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
			string Code = null;
			if (dt.Rows.Count > 0)
			{
				Code = dt.Rows[0]["Id"].ToString();
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(ReportsParameters obj)
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
				ParameterName = "@RefDocumentTypeIdgpi",
				Value = obj.RefDocumentTypeIdgpi
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeIdgpo",
				Value = obj.RefDocumentTypeIdgpo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeIdMo",
				Value = obj.RefDocumentTypeIdMo
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DocumentTypeForManualWages(int OrganizationId, int CompanyId, int BranchesId = 0)
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
			if (BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DocumentTypeForManualWages"
			});
			return GenericProvider.GetDataTableProc<GatepassType>("[dbo].[Sp_InvContractorWagesBillHeader_GetAllMethod]", p);
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetHistory(ReportsParameters obj)
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
			_ = obj.FromDate;
			p.Add(new SqlParameter
			{
				ParameterName = "@BillDateFrom",
				Value = obj.FromDate
			});
			_ = obj.ToDate;
			p.Add(new SqlParameter
			{
				ParameterName = "@BillDateTo",
				Value = obj.ToDate
			});
			if (obj.FromDocNo > 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillSrFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo > 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillSrTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.CwCoaId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CwCoaId",
					Value = obj.CwCoaId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@HeaderId",
				Value = obj.HeaderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "BillHistroy_Rep"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractorWagesBill_SlipandRegister(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			if (obj.RefRefDocIdNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocNoId",
					Value = obj.RefRefDocIdNo
				});
			}
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvConractorWagesAccountsId",
					Value = obj.AccountId
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
					ParameterName = "@BillDateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BillDateTo",
					Value = obj.ToDate
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FreeOfCost",
					Value = obj.IsApproved
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
			if (obj.StockPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StockPartyId",
					Value = obj.StockPartyId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_SlipandRegister", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetIdByRefDocTypeIdAndRefDocId(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocNoId",
				Value = obj.RefDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.RefDocument != null && obj.RefDocument != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = obj.RefDocument
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByRefDocTypeIdAndRefDocId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractorWagesBillHeader_ReadPreviousRecords(Architecture.Model.ContractorWages.InvContractorWagesBillHeader obj)
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
				ParameterName = "@RefDocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocNoId",
				Value = obj.RefDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("[dbo].[USP_ContractorWagesBillHeader_ReadPreviousRecords]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWagesAccounts(ReportsParameters obj)
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
				Value = "GetWagesAccounts"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetActivityNature(ReportsParameters obj)
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("USP_DropDownActivityWages", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWagesContractors(ReportsParameters obj)
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
				Value = "GetWagesContractors"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockPartyFromWages(ReportsParameters obj)
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
				Value = "GetStockPartyFromWages"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetJobOrderFromWages(ReportsParameters obj)
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
				Value = "GetJobOrderFromWages"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDocumentTypesFromContractorWages(ReportsParameters obj)
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
				Value = "GetDocumentTypesFromContractorWages"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("Sp_InvContractorWagesBillHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadWagesSummaryByDocumentTypeAndDocumentId(int DocumentTypeId, int DocumentId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = DocumentId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadWagesSummaryByDocumentTypeAndDocumentId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("[dbo].[Sp_InvContractorWagesBillHeader_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet ContractorWagesDashboardsandReport(ReportsParameters obj)
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
					ParameterName = "@DateFrom",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DateTo",
					Value = obj.ToDate
				});
			}
			if (obj.ContractorId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContratorId",
					Value = obj.ContractorId
				});
			}
			if (obj.WagesItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WagesAccountId",
					Value = obj.WagesItemId
				});
			}
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("USP_ContractorWagesDashboardsandReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryNew(ReportsParameters obj)
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
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
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("[dbo].[USP_ContractorWagesBillHeader_FormHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSummaryWagesByRefDocumentsAndActivities(ReportsParameters obj)
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvConractorWagesAccountsId",
					Value = obj.AccountId
				});
			}
			if (!string.IsNullOrEmpty(obj.BranchesIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FreeOfCost",
					Value = obj.IsApproved
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
			if (obj.StockPartyId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StockPartyId",
					Value = obj.StockPartyId
				});
			}
			if (obj.SkipZero != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchWise",
					Value = obj.SkipZero
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("USP_GetSummaryWagesByRefDocumentsAndActivities", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvContractorWagesBillHeader_DetailByRefDocument(int OrganizationId, int CompanyId, int FinancialYearId, int RefDocumentTypeId, int RefDocId)
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
				ParameterName = "@FinancialYearId",
				Value = FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocId ",
				Value = RefDocId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("USP_InvContractorWagesBillHeader_DetailByRefDocument", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void WagesDeleteByRefDocTypeAndId(int OrganizationId, int CompanyId, int RefDocumentTypeId, int RefDocId)
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
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefDocId ",
				Value = RefDocId
			});
			GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("USP_WagesDeleteByRefDocTypeAndId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetRefDocumentsForWages(int RefDocumentTypeId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			if (RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = RefDocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.ContractorWages.InvContractorWagesBillHeader>("USP_GetRefDocumentsForWages", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WagesTypeIdsAgainstDocumentType_GetAll()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<GatepassType>("[dbo].[USP_WagesTypeIdsAgainstDocumentType_GetAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateContractorWagesRefDocumentStatus(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = item.DocumentTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyDate",
					Value = item.PostDate
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyUserId",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@IsActive",
					Value = item.Status
				});
				GenericProvider.GetDataTableProc<VoucherHead>("USP_ContractorWagesRefDocumentStatus_Insert", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ComboAgainstContractorWages(ReportsParameters obj)
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
			if (obj.Activity != null && obj.Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<InvGdn>("Usp_AllComboAgainstContractorWages", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable JobOrderNoFromContractorWages(ReportsParameters obj)
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
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			return GenericProvider.GetDataTableProc<InvGdn>("Usp_JobOrderNoFromContractorWages", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ContractorWagesRemovebyReferenceIds(int OrganizationId, int CompanyId, int DocumentTypeId, int Id)
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			GenericProvider.GetDataTableProc<InvGdn>("usp_ContractorWagesRemovebyReferenceIds", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContractorWagesDataForApproval(ReportsParameters obj)
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
			if (obj.BranchesIds != null && obj.BranchesIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<InvGdn>("usp_getContractorWagesForApproval", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ContractorWagesApprovalStatus_Update(ApprovalList obj)
	{
		try
		{
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			SqlTransaction sqlTrn = sqlcon.BeginTransaction();
			try
			{
				foreach (ApprovalList item in obj.AllApprovalLists)
				{
					new List<SqlParameter>();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlcon;
					sqlCommand.Transaction = sqlTrn;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandText = "usp_ContractorWagesApprovalStatus_Update";
					sqlCommand.Parameters.AddWithValue("@Id", item.Id);
					sqlCommand.Parameters.AddWithValue("@UserId", item.EntryUser);
					sqlCommand.ExecuteNonQuery();
				}
				sqlTrn.Commit();
			}
			catch (Exception ex)
			{
				sqlTrn.Rollback();
				throw ex;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	public static void UpdateContractorWagesRefDocumentStatus(ReportsParameters obj)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ReportsParameters item in obj.ReportsParameterslist)
			{
				using SqlCommand sqlCommandPS = new SqlCommand
				{
					Connection = sqlcon,
					Transaction = sqlTrn,
					CommandType = CommandType.StoredProcedure,
					CommandText = "[dbo].[USP_ContractorWagesCancelRecords_InsertAndUpdate]"
				};
				sqlCommandPS.Parameters.AddWithValue("@Id", 0);
				sqlCommandPS.Parameters.AddWithValue("@RefDocumentTypeId", item.RefDocumentTypeId);
				sqlCommandPS.Parameters.AddWithValue("@RefDocId", item.RefDocIdNo);
				sqlCommandPS.Parameters.AddWithValue("@OrganizationId", item.OrganizationId);
				sqlCommandPS.Parameters.AddWithValue("@CompanyId", item.CompanyId);
				sqlCommandPS.Parameters.AddWithValue("@EntryUserId", item.EntryUser);
				sqlCommandPS.Parameters.AddWithValue("@ModifyUserId", item.ModifyUserId);
				sqlCommandPS.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw new Exception("Transaction rolled back due to an error: " + ex.Message, ex);
		}
	}
}
