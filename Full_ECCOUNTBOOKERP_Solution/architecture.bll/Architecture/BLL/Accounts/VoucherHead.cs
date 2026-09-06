using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Common;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class VoucherHead
{
	public static int Save(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.DocumentTypeId == 1 || obj.DocumentTypeId == 2 || obj.DocumentTypeId == 25 || obj.DocumentTypeId == 26 || obj.DocumentTypeId == 27)
			{
				obj.Type = VoucherType.Payment;
			}
			if (obj.DocumentTypeId == 6)
			{
				obj.Type = VoucherType.JournalCredit;
			}
			if (obj.DocumentTypeId == 24)
			{
				obj.Type = VoucherType.Receipt;
			}
			if (obj.DocumentTypeId == 10)
			{
				obj.Type = VoucherType.Contra;
			}
			if (obj.DocumentTypeId == 3 || obj.DocumentTypeId == 4)
			{
				obj.Type = VoucherType.Receipt;
			}
			if (obj.DocumentTypeId == 7)
			{
				obj.Type = VoucherType.JournalDebit;
			}
			if (obj.Type == VoucherType.Payment || obj.Type == VoucherType.JournalCredit || obj.Type == VoucherType.Contra)
			{
				if (obj.DocumentTypeId == 25)
				{
					double VoucherAmount2 = 0.0;
					foreach (VoucherDetail VD5 in obj.voucherDetailList.ToList())
					{
						VD5.DebitAmount = VD5.DebitAmount;
						VD5.CreditAmount = 0.0;
						VD5.IsTaxable = "False";
						VoucherAmount2 += VD5.DebitAmount;
						new Architecture.DAL.Accounts.VoucherHead().VoucherDetailIdUpdate(VD5.InvoiceNoRefId);
					}
					obj.VoucherAmount = VoucherAmount2;
					foreach (VoucherDetail VD6 in obj.voucherDetailList.ToList())
					{
						VoucherDetail vd3 = new VoucherDetail();
						vd3.CreditAmount = VD6.DebitAmount;
						vd3.DebitAmount = 0.0;
						vd3.AgainstAccountId = obj.AgainstAccountId;
						vd3.AccountId = obj.RefAccountId;
						vd3.JobLotId = VD6.JobLotId;
						vd3.Comments = VD6.Comments;
						vd3.IsTaxable = "False";
						obj.voucherDetailList.Add(vd3);
					}
				}
				if (obj.DocumentTypeId == 27)
				{
					double VoucherAmount3 = 0.0;
					bool FinancialisActiveonCommercialInvoice = Conversion.ToBool(CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "FinancialisActiveonCommercialInvoice"));
					List<GetItemGlIdsandItemName> dtitem = new List<GetItemGlIdsandItemName>();
					dtitem = CommonServices.GetItemGlIdsandItemName(obj.OrganizationId, obj.CompanyId);
					foreach (VoucherDetail VD4 in obj.voucherDetailList.ToList())
					{
						VD4.CreditAmount = VD4.CreditAmount;
						VD4.IsTaxable = "False";
						VoucherAmount3 += VD4.CreditAmount;
					}
					obj.VoucherAmount = VoucherAmount3;
					foreach (VoucherDetail VD in obj.voucherDetailList.ToList())
					{
						if (FinancialisActiveonCommercialInvoice)
						{
							ReportsParameters cgs2 = new ReportsParameters();
							cgs2.OrganizationId = obj.OrganizationId;
							cgs2.CompanyId = obj.CompanyId;
							cgs2.DocDate = obj.VoucherDate;
							cgs2.ItemId = VD.ItemId;
							cgs2.JobLotId = VD.JobLotId;
							if (obj.Id > 0)
							{
								cgs2.DocumentTypeId = obj.DocumentTypeId;
								cgs2.Id = obj.DocumentTypeSrNo;
							}
							VD.ItemCgsRate = CommonServies.AvgRateOnlyForCGS(cgs2);
							VD.CreditAmount = VD.ItemCgsRate * VD.WeightOut;
							VD.ItemAmount = VD.CreditAmount;
						}
						VoucherDetail vd2 = new VoucherDetail();
						vd2.AccountId = obj.RefAccountId;
						vd2.AgainstAccountId = VD.AccountId;
						vd2.DebitAmount = VD.CreditAmount;
						vd2.JobLotId = VD.JobLotId;
						vd2.OrderNo = VD.OrderNo;
						vd2.Comments = VD.Comments;
						vd2.IsTaxable = "False";
						vd2.QtyOut = VD.QtyOut;
						vd2.WeightOut = VD.WeightOut;
						vd2.ItemRate = VD.ItemRate;
						vd2.ItemId = VD.ItemId;
						vd2.ItemAmount = VD.ItemAmount;
						vd2.DMultiCurrencyId = VD.DMultiCurrencyId;
						vd2.DExchangeCurrencyRate = VD.DExchangeCurrencyRate;
						vd2.DCurrencyAmount = VD.DCurrencyAmount;
						vd2.SupplierCustomerId = VD.SupplierCustomerId;
						obj.voucherDetailList.Add(vd2);
						if (FinancialisActiveonCommercialInvoice)
						{
							continue;
						}
						bool CgsAllow = false;
						string ConfigKey = CommonServies.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "CGSEntryAllow");
						if (ConfigKey != string.Empty)
						{
							CgsAllow = Conversion.ToBool(ConfigKey);
						}
						double CgsAmount = 0.0;
						string RemarksCgs = "";
						if (CgsAllow)
						{
							ReportsParameters cgs = new ReportsParameters();
							cgs.OrganizationId = obj.OrganizationId;
							cgs.CompanyId = obj.CompanyId;
							cgs.DocDate = obj.VoucherDate;
							cgs.ItemId = VD.ItemId;
							cgs.JobLotId = VD.JobLotId;
							if (obj.Id > 0)
							{
								cgs.DocumentTypeId = obj.DocumentTypeId;
								cgs.Id = obj.Id;
							}
							VD.ItemCgsRate = CommonServies.AvgRateOnlyForCGS(cgs);
						}
						if (VD.ItemCgsRate > 0.0)
						{
							CgsAmount = VD.WeightOut * VD.ItemCgsRate;
							List<GetItemGlIdsandItemName> find = dtitem.Where((GetItemGlIdsandItemName x) => x.Id == Conversion.ToInt(VD.ItemId)).ToList();
							RemarksCgs = "Item: " + Conversion.ToString(find[0].ItemName) + ",   Qty: " + VD.QtyOut + ",   Weight: " + VD.WeightOut + ",   Rate:" + VD.ItemCgsRate + "  Item Amount: " + CgsAmount;
							VoucherDetail vd5 = new VoucherDetail();
							if (CgsAllow)
							{
								vd5.AccountId = Conversion.ToInt(find[0].COGSGLAC);
							}
							else
							{
								vd5.AccountId = obj.RefDocNoId;
							}
							vd5.AgainstAccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							vd5.Comments = Conversion.ToString(RemarksCgs);
							vd5.DebitAmount = Conversion.ToDouble(CgsAmount);
							vd5.ItemId = Conversion.ToInt(VD.ItemId);
							vd5.JobLotId = Conversion.ToInt(VD.JobLotId);
							vd5.OrderNo = Conversion.ToInt(VD.OrderNo);
							vd5.QtyOut = Conversion.ToDouble(VD.QtyOut);
							vd5.WeightOut = Conversion.ToDouble(VD.WeightOut);
							vd5.ItemRate = Conversion.ToDouble(VD.ItemCgsRate);
							vd5.ItemCgsRate = Conversion.ToDouble(VD.ItemCgsRate);
							vd5.ItemAmount = Conversion.ToDouble(CgsAmount);
							vd5.SupplierCustomerId = Conversion.ToInt(obj.RefAccountId);
							vd5.RefDocSubIdNo = Conversion.ToInt(VD.RefDocSubIdNo);
							obj.voucherDetailList.Add(vd5);
							VoucherDetail vd4 = new VoucherDetail();
							vd4.AccountId = Conversion.ToInt(find[0].PurchaseGLAC);
							if (CgsAllow)
							{
								vd4.AgainstAccountId = Conversion.ToInt(find[0].COGSGLAC);
							}
							else
							{
								vd4.AgainstAccountId = obj.RefDocNoId;
							}
							vd4.Comments = Conversion.ToString(RemarksCgs);
							vd4.CreditAmount = Conversion.ToDouble(CgsAmount);
							vd4.ItemId = Conversion.ToInt(VD.ItemId);
							vd4.JobLotId = Conversion.ToInt(VD.JobLotId);
							vd4.OrderNo = Conversion.ToInt(VD.OrderNo);
							vd4.QtyOut = Conversion.ToDouble(VD.QtyOut);
							vd4.WeightOut = Conversion.ToDouble(VD.WeightOut);
							vd4.ItemRate = Conversion.ToDouble(VD.ItemCgsRate);
							vd4.ItemCgsRate = Conversion.ToDouble(VD.ItemCgsRate);
							vd4.ItemAmount = Conversion.ToDouble(CgsAmount);
							vd4.SupplierCustomerId = Conversion.ToInt(obj.RefAccountId);
							vd4.RefDocSubIdNo = Conversion.ToInt(VD.RefDocSubIdNo);
							obj.voucherDetailList.Add(vd4);
							VD.RefDocSubIdNo = 0;
							VD.ItemCgsRate = 0.0;
							VD.WeightIn = 0.0;
						}
					}
				}
			}
			if ((obj.Type == VoucherType.Receipt || obj.Type == VoucherType.JournalDebit) && obj.DocumentTypeId == 24)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "VoucherDetail_ReadByVoucherHeadID"
				});
				DataTable lst = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
				if (lst.Rows.Count > 0)
				{
					for (int i = 0; i < lst.Rows.Count; i++)
					{
						if (lst.Rows[i]["RefInvoiceNo"].ToString() == "PDC")
						{
							int RefId = Conversion.ToInt(lst.Rows[i]["InvoiceNoRefId"]);
							new Architecture.DAL.Accounts.PdcInventory().PdcCheqStatusUpdate(RefId, "Pending");
						}
					}
				}
				double VoucherAmount = 0.0;
				foreach (VoucherDetail VD3 in obj.voucherDetailList.ToList())
				{
					VD3.CreditAmount = VD3.CreditAmount;
					VD3.DebitAmount = 0.0;
					VD3.IsTaxable = "False";
					VoucherAmount += VD3.CreditAmount;
					new Architecture.DAL.Accounts.PdcInventory().PdcCheqStatusUpdate(VD3.InvoiceNoRefId, "Clear");
				}
				obj.VoucherAmount = VoucherAmount;
				foreach (VoucherDetail VD2 in obj.voucherDetailList.ToList())
				{
					VoucherDetail vd1 = new VoucherDetail();
					vd1.AccountId = obj.RefAccountId;
					vd1.AgainstAccountId = VD2.AccountId;
					vd1.SupplierCustomerId = VD2.SupplierCustomerId;
					vd1.Comments = VD2.Comments;
					if (VD2.InvoiceNoRefId > 0)
					{
						vd1.InvoiceNoRefId = VD2.InvoiceNoRefId;
					}
					vd1.CheqNoDetail = VD2.CheqNoDetail;
					vd1.DCheqDate = VD2.DCheqDate;
					vd1.CommentsOtherLingo = VD2.CommentsOtherLingo;
					vd1.DebitAmount = VD2.CreditAmount;
					vd1.IsTaxable = "False";
					obj.voucherDetailList.Add(vd1);
				}
			}
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.Accounts.VoucherHead().SetData(obj, "Sp_VoucherHead_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.Accounts.VoucherHead().SetData(obj, "Sp_VoucherHead_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void VoucherDetailUpdate(int Id)
	{
		try
		{
			new Architecture.DAL.Accounts.VoucherHead().VoucherDetailIdUpdate(Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllVoucherForChallanDeposit(Architecture.Model.Accounts.VoucherHead obj)
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
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ProjectsId",
				Value = obj.ProjectId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefAccountId",
				Value = obj.RefAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPendingWhtChallan"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GenerateVoucherCodeByDocumentTypeId(Architecture.Model.Accounts.VoucherHead obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateVoucherCodeByDocumentTypeId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.VoucherHead GetByID(int Id)
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
				Value = "ReadByID"
			});
			return new Architecture.DAL.Accounts.VoucherHead().GetData("Sp_Vouchers_GetMethods", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = Conversion.ToInt(obj.CompanyId)
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetVoucherHeadIdByVoucherCode(Architecture.Model.Accounts.VoucherHead obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVoucherHeadIdByVoucherCode"
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherCode",
				Value = obj.VoucherCode
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
			if (dt.Rows.Count > 0 && dt != null)
			{
				code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInstrumentTypes()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("usp_getInstrumentType", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByCurrentBalanceByDateAndAccountId(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByCurrentBalanceByDateAndAccountId"
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefAccountId",
				Value = obj.RefAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherDate",
				Value = obj.VoucherDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGLAndSubsidiaryCurrentBalance(ReportsParameters obj)
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
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@GlAccountId",
				Value = obj.AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SubSaidiaryId",
				Value = obj.SupplierCustomerId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("USP_GetGLAndSubsidiaryCurrentBalance", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetVoucherHeadIdByDocumentTypeIdandRefDocNoId(Architecture.Model.Accounts.VoucherHead obj)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeSrNo",
				Value = obj.DocumentTypeSrNo
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
			if (dt.Rows.Count > 0 && dt != null)
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

	public static DataTable VoucherFormHistory(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeName",
				Value = obj.Ids
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.PostState
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
				});
			}
			if (!obj.PostState)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved ",
					Value = obj.IsApproved
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
			if (obj.AppId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AppId",
					Value = obj.AppId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("USP_VoucherFormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetReceivablesByInvoice(ReportsParameters obj)
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
			ParameterName = "@SupplierCustomerId",
			Value = obj.SupplierCustomerId
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
		if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDateFrom",
				Value = obj.DueDateFrom
			});
		}
		if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDateTo",
				Value = obj.DueDateTo
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
		return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_PaymentByInvoiceHeader_GetReceivablesByInvoice", p);
	}

	public static DataTable Receivables(ReportsParameters obj)
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
					ParameterName = "@VoucherDateF",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDateT",
					Value = obj.ToDate
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Receivables_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable RecordCheckTables(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_CheckIsDocumentNoExistInReferenceTables", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateStatusandIsapprovedByVoucherId(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "UpdateStatusandIsapprovedByVoucherId"
				});
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
					ParameterName = "@Id",
					Value = item.Id
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@PostDate",
					Value = DateTime.Now
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = item.EntryUser
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionTypeId",
					Value = item.ActionTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ReqType",
					Value = item.ReqType
				});
				GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherForApprovalDashboard(ReportsParameters obj)
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
			if (obj.UserId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@UserId",
					Value = obj.UserId
				});
			}
			if (obj.PendingForViewFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingForViewId",
					Value = obj.ActionTypeId
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
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("USp_PendingVoucherForApprovalDashboard", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingVoucherReconcilationForApprovalDashboard(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeName",
				Value = Conversion.ToString(obj.DocumentTypeId)
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PendingVoucherReconcilationForApprovalDashboard"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingVoucherForUnApprovalDashboard(ReportsParameters obj)
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
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PendingVoucherForUnApprovalDashboard"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DocumentTypeGetFromInventoryTransactions(Architecture.Model.Accounts.VoucherHead obj)
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
				Value = "DocumentTypeGetFromInventoryTransactions"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PendingForAccountsGLEntry(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrginzationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (obj.EximInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EximInvoiceId",
					Value = obj.EximInvoiceId
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ExImLcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImLcOrderId",
					Value = obj.ExImLcOrderId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_ExImInvoice_PendingForAccountsGLEntry", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetReceivablesByInvoiceReceipt(ReportsParameters obj)
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
		if (obj.SaleInvoiceDocumentTypeIds != string.Empty)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleInvoiceDocumentTypeIds",
				Value = obj.SaleInvoiceDocumentTypeIds
			});
		}
		p.Add(new SqlParameter
		{
			ParameterName = "@SupplierCustomerId",
			Value = obj.SupplierCustomerId
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
		if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDateFrom",
				Value = obj.DueDateFrom
			});
		}
		if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDateTo",
				Value = obj.DueDateTo
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
		return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_ReceiptByInvoiceHeader_GetReceivablesByInvoice", p);
	}

	public static DataTable GetDetailByVoucherHeadId(int Id)
	{
		try
		{
			new DataTable();
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDetailByVoucherHeadId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDebitAcBySupplierCustomerGLAcIdForChallanDeposit(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDebitAcBySupplierCustomerGLAcIdForChallanDeposit"
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
				ParameterName = "@RefAccountId",
				Value = obj.RefAccountId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayablesandReceivablesMenagment(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EndDate",
					Value = obj.ToDate
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IntervalDays",
					Value = obj.IntervalDays
				});
			}
			if (obj.DocNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CBalNotEqualTo",
					Value = obj.DocNo
				});
			}
			if (obj.ItemClassId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportClassification",
					Value = obj.ItemClassId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReportType",
					Value = obj.ItemTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("SpAccounts_PayablesReceivablesAging_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoByPaymentByInvoice(ReportsParameters obj)
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
				ParameterName = "@RefAccountId",
				Value = obj.AccountId
			});
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoiceNoByPaymentByInvoice"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceWiseBalanceAmount(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefAccountId",
				Value = obj.AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoiceWiseBalanceAmount"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PaymentByInvoiceVoucherNewHistory(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "PaymentByInvoiceVoucherNewHistory"
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
			if (Conversion.ToInt(obj.DocumentTypeId) == 1 || Conversion.ToInt(obj.DocumentTypeId) == 2)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "1,2"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 3 || Conversion.ToInt(obj.DocumentTypeId) == 4)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "3,4"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 25)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "25"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 26)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "26"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 27)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "27"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 24)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "24"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 6)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "6"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 7)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "7"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 10)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "10"
				});
			}
			if (Conversion.ToInt(obj.DocumentTypeId) == 5)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeName",
					Value = "5"
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.PostState
			});
			if (!obj.PostState)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
				});
			}
			if (obj.VoucherCode != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.VoucherCode
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesForPaymentInvoiceVoucherLoader(ReportsParameters obj)
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
				ParameterName = "@RefAccountId",
				Value = obj.AccountId
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
			if (obj.PurchaseInvoiceIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InvoiceIds",
					Value = obj.PurchaseInvoiceIds
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@ReqType",
				Value = obj.ReqType
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetInvoicesForPaymentInvoiceVoucherLoader"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAdvanceAmountByAccountId(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAdvanceAmountByAccountId"
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@RefAccountId",
				Value = obj.RefAccountId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetCurrentGLAccountBalance(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			double CurrentBalance = 0.0;
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
				ParameterName = "@RefAccountId",
				Value = obj.RefAccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherDate",
				Value = obj.VoucherDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetCurrentGLAccountBalance"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
			if (dt.Rows.Count > 0)
			{
				CurrentBalance = Conversion.ToDouble(dt.Rows[0]["CurrentBalance"]);
			}
			return CurrentBalance;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVouchersMainAndDetailForApproval(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVouchersMainAndDetailForApproval"
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
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
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
			if (obj.ApprovedFilter != null && obj.ApprovedFilter != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DayBookHistory(int OrganizationId, int CompanyId, int FinancialYearId, DateTime VoucherDate, int CashAccountId = 0, int DocumentTypeId = 0)
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
			if (CashAccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChartOfAccountId",
					Value = CashAccountId
				});
			}
			if (DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = DocumentTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@VoucherDate",
				Value = VoucherDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DayBookHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountsBalancesOpeningCurrentAndClosing(ReportsParameters obj)
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
				ParameterName = "@AccountId",
				Value = obj.AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = obj.ToDate
			});
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CostCenterId",
					Value = obj.CostCenterId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAccountsBalancesOpeningCurrentAndClosing"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSubsidiaryLedgerBalance(int OrganizationId, int CompanyId, int SupplierCustomerId, DateTime ToDate)
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
				ParameterName = "@ToDate",
				Value = ToDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("[dbo].[USP_Accounts_GetSubsidiaryLedgerBalance]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDocumentTypesFromVouchers(Architecture.Model.Accounts.VoucherHead obj)
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
				Value = "GetDocumentTypesFromVouchers"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPartiesFromVouchersByDate(int OrganizationId, int CompanyId, DateTime DocDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@ToDate", DocDate));
			p.Add(new SqlParameter("@Activity", "GetPartiesFromVouchersByDate"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetCashAndBanksAccountByDate(int OrganizationId, int CompanyId, DateTime DocDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@ToDate", DocDate));
			p.Add(new SqlParameter("@Activity", "GetCashAndBanksAccountByDate"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemByStockAccount(int OrganizationId, int CompanyId, int AccountId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@AccountId", AccountId));
			p.Add(new SqlParameter("@Activity", "GetItemByStockAccount"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReceiptByContract_FromHistory(ReportsParameters obj)
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
		if (obj.SupplierCustomerId != 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
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
		return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("USp_ReceiptByContract_FromHistory", p);
	}

	public static DataTable GetPendingOrderForReceiptsVoucher(ReportsParameters obj)
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
		if (obj.SupplierCustomerId != 0)
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@SupplierCustomerId",
				Value = obj.SupplierCustomerId
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
		if (!Conversion.CheckDateTimeNull(obj.DueDateFrom))
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDateFrom",
				Value = obj.DueDateFrom
			});
		}
		if (!Conversion.CheckDateTimeNull(obj.DueDateTo))
		{
			p.Add(new SqlParameter
			{
				ParameterName = "@DueDateTo",
				Value = obj.DueDateTo
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
		return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("USP_GetPendingOrderForReceiptsVoucher", p);
	}

	public static DataTable BindSubsidiaryAccount(int CompanyId, int DetailAccountId = 0, string TypeId = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@SubsidiaryTypeIds", TypeId));
			if (DetailAccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", DetailAccountId));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_GetSubsidiaryAccountsByParentAccount", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSubsidiaryAccountFromVouchers(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", obj.OrganizationId));
			p.Add(new SqlParameter("@CompanyId", obj.CompanyId));
			p.Add(new SqlParameter("@UserId", obj.UserId));
			p.Add(new SqlParameter("@AppId", obj.AppId));
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", obj.AccountId));
			}
			if (obj.CostCenterId != 0)
			{
				p.Add(new SqlParameter("@CostCenterId", obj.CostCenterId));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_GetSubsidiaryAccountFromVouchers", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBranchesFromVouchersByAccountId(int OrganizationId, int CompanyId = 0, string CompanyIds = "", int AccountId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			if (CompanyId != 0)
			{
				p.Add(new SqlParameter("@CompanyId", CompanyId));
			}
			if (CompanyIds != null && CompanyIds != string.Empty)
			{
				p.Add(new SqlParameter("@CompanyIds", CompanyIds));
			}
			if (AccountId != 0)
			{
				p.Add(new SqlParameter("@AccountId", AccountId));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_GetBranchesFromVouchersByAccountId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InsertBookmarkStatus(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@BookmarkStatus",
					Value = item.ActionTypeId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@BookmarkRemarks",
					Value = item.Comments
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = item.Id
				});
				GenericProvider.GetDataTableProc<VoucherDetail>("USP_InsertBookmarkStatus", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InsertBookmarkStatusInBulk(ApprovalList obj)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("BookmarkStatus", typeof(bool));
		dt.Columns.Add("BookmarkRemarks", typeof(string));
		foreach (ApprovalList item in obj.AllApprovalLists)
		{
			dt.Rows.Add(item.Id, item.ActionTypeId, item.Comments);
		}
		using SqlConnection con = ConnectionObject.SQL();
		con.Open();
		using SqlTransaction transaction = con.BeginTransaction();
		try
		{
			using (SqlCommand sqlCommand = new SqlCommand("\r\n                        IF OBJECT_ID('tempdb..#TempBookmark') IS NOT NULL\r\n                        DROP TABLE #TempBookmark", con, transaction))
			{
				sqlCommand.ExecuteNonQuery();
			}
			using (SqlCommand sqlCommand2 = new SqlCommand("\r\n                        CREATE TABLE #TempBookmark\r\n                        (\r\n                            Id INT,\r\n                            BookmarkStatus BIT,\r\n                            BookmarkRemarks NVARCHAR(MAX)\r\n                        )", con, transaction))
			{
				sqlCommand2.ExecuteNonQuery();
			}
			using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, transaction))
			{
				bulkCopy.DestinationTableName = "#TempBookmark";
				bulkCopy.WriteToServer(dt);
			}
			using (SqlCommand cmd = new SqlCommand("\r\n                        UPDATE vd\r\n                        SET \r\n                        vd.BookmarkStatus = t.BookmarkStatus,\r\n                        vd.BookmarkRemarks = t.BookmarkRemarks\r\n                        FROM VoucherDetail vd\r\n                        INNER JOIN #TempBookmark t\r\n                        ON vd.Id = t.Id", con, transaction))
			{
				cmd.ExecuteNonQuery();
			}
			transaction.Commit();
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}

	public static DataTable GetSubsidiaryPayablesReceivables(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.UserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DateFrom",
				Value = obj.FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DateTo",
				Value = obj.ToDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ControlAccountId ",
				Value = obj.AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PartyTypeId ",
				Value = obj.AccountTypeId
			});
			return GenericProvider.GetDataTableProc<VoucherDetail>("USP_GetSubsidiaryPayablesReceivables", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool VoucherExistWithSameAmountInSameDate(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			foreach (VoucherDetail detail in (from x in obj.voucherDetailList
				where x.DebitAmount > 0.0
				group x by x.AccountId into g
				select g.First()).ToList())
			{
				List<SqlParameter> s = new List<SqlParameter>();
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				if (detail.AccountId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@AccountId",
						Value = detail.AccountId
					});
				}
				if (!Conversion.CheckDateTimeNull(obj.VoucherDate))
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@VoucherDate",
						Value = obj.VoucherDate
					});
				}
				s.Add(new SqlParameter
				{
					ParameterName = "@DebitAmount",
					Value = detail.DebitAmount
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = detail.ActionId
				});
				DataTable dt = GenericProvider.GetDataTableProc<SaleOrder>("[dbo].[USP_VoucherExistWithSameAmountInSameDate]", s);
				if (dt != null && dt.Rows.Count > 0 && MessageBox.Show("Voucher against '" + Conversion.ToString(dt.Rows[0]["Title"]) + "' with same Debit Amount already exists on this date. Do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return false;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static double GetGLBalanceByAccountTypeIdClassIdAndAccountId(int OrganizationId, int CompanyId, int AccountId, DateTime ToDate, int AccountTypeId, int AccountClass)
	{
		try
		{
			double Amount = 0.0;
			List<SqlParameter> s = new List<SqlParameter>();
			s.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@AccountId",
				Value = AccountId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@AccountTypeId",
				Value = AccountTypeId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@AccountClass",
				Value = AccountClass
			});
			DataTable dt = GenericProvider.GetDataTableProc<SaleOrder>("[dbo].[USP_GetGLBalanceByAccountTypeIdClassIdAndAccountId]", s);
			if (dt.Rows.Count > 0)
			{
				Amount = Conversion.ToDouble(dt.Rows[0][0]);
			}
			return Amount;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable DayBookApprovalHistory(ReportsParameters obj)
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ChartOfAccountId",
					Value = obj.AccountId
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
			if (!Conversion.CheckDateTimeNull(obj.DocDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VoucherDate",
					Value = obj.DocDate
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("[dbo].[USP_DayBookApprovalHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLastExchangeRateAndCurrencyOfVoucher(ReportsParameters obj)
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
			if (obj.DocumentTypeIds != null && obj.DocumentTypeIds != string.Empty)
			{
				p.Add(new SqlParameter("@DocumentTypeIds", obj.DocumentTypeIds));
			}
			if (obj.Ids != null && obj.Ids != string.Empty)
			{
				p.Add(new SqlParameter("@DMultiCurrencyIds", obj.Ids));
			}
			p.Add(new SqlParameter("@Activity", "GetMultiCurrencyAndLastRate"));
			return GenericProvider.GetDataTableProc<VoucherDetail>("Sp_Vouchers_GetMethods", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountTypesForBankSummary(int OrganizationId, int CompanyId, int AccountTypeId)
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
				ParameterName = "@AccountTypeId",
				Value = AccountTypeId
			});
			return GenericProvider.GetDataTableProc<VoucherDetail>("USP_AccountTypesForBankSummary", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getPostDatedCheqByAccountId(int OrganizationId, int CompanyId, int AccountId, DateTime ToDate)
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
				ParameterName = "@AccountId",
				Value = AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			return GenericProvider.GetDataTableProc<VoucherDetail>("usp_getPostDatedCheqByAccountId", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FinancialYearCloseRun(int OrganizationId, int CompanyId, DateTime FromDate, DateTime ToDate, int BranchesId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@FromDate", FromDate));
			p.Add(new SqlParameter("@ToDate", ToDate));
			if (BranchesId != 0)
			{
				p.Add(new SqlParameter("@BranchesId", BranchesId));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.ChartofAccount>("USP_FinancialYearClose", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLocationType()
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			return GenericProvider.GetDataTableProc<VoucherDetail>("usp_getLocationType", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Accounts.VoucherHead GetVoucherHeaderByID(int Id)
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
				Value = "ReadByID"
			});
			return new Architecture.DAL.Accounts.VoucherHead().GetDateAll("Sp_Vouchers_GetMethods", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Voucher_PendingForUpload(ReportsParameters obj)
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
				ParameterName = "@UserId",
				Value = obj.UserId
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
			if (obj.AccountId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@AccountId",
					Value = obj.AccountId
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved ",
					Value = obj.IsApproved
				});
			}
			if (!string.IsNullOrWhiteSpace(obj.DocumentTypeIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.VoucherHead>("[dbo].[USP_Voucher_PendingForUpload]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateVoucherUploadStatus(int UploadedById, int Id, bool IsUploaded = true)
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
			sqlCommand.CommandText = "[dbo].[USP_VoucherHead_UpdateVoucherUploadStatus]";
			sqlCommand.Parameters.AddWithValue("@UploadedById", UploadedById);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.Parameters.AddWithValue("@IsUploaded", IsUploaded);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable GetVouchersDateForDaybookDropdown(int OrganizationId, int CompanyId, int DocumentTypeId)
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
			return GenericProvider.GetDataTableProc<VoucherDetail>("usp_getvouchersdateforDaybookDropdown", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ChartOfAccountParentUpdate(int OrganizationId, int CompanyId, int UserId, int ChartOfAccountId, int ParentAccountIdOrToChartOfAccountId, int ActionId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommandInv = new SqlCommand
			{
				Connection = sqlcon,
				Transaction = sqlTrn,
				CommandType = CommandType.StoredProcedure,
				CommandText = "[dbo].[usp_AccountMoveToAnotherParentGroup]"
			};
			sqlCommandInv.Parameters.AddWithValue("@OrganizationId", OrganizationId);
			sqlCommandInv.Parameters.AddWithValue("@CompanyId", CompanyId);
			sqlCommandInv.Parameters.AddWithValue("@UserId", UserId);
			sqlCommandInv.Parameters.AddWithValue("@ChartOfAccountId", ChartOfAccountId);
			sqlCommandInv.Parameters.AddWithValue("@ActionId", ActionId);
			switch (ActionId)
			{
			case 1:
				sqlCommandInv.Parameters.AddWithValue("@ParentAccountId", ParentAccountIdOrToChartOfAccountId);
				break;
			case 2:
				sqlCommandInv.Parameters.AddWithValue("@ToChartOfAccountId", ParentAccountIdOrToChartOfAccountId);
				break;
			}
			sqlCommandInv.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}
}
