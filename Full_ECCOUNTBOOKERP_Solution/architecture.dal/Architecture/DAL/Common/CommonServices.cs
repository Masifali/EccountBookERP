using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.PartyProcessing;
using Architecture.Model.VPS;

namespace Architecture.DAL.Common;

public class CommonServices : DALBase
{
	private static readonly char[] AlphabetsArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

	private static readonly char[] NumbersArray = "0123456789".ToCharArray();

	public static void ReplaceCompLogoColumn(DataTable dtToChange, DataTable dtData)
	{
		try
		{
			if (dtToChange == null || dtData == null || dtData.Rows.Count == 0)
			{
				return;
			}
			if (dtToChange.Columns.Contains("CompLogoImage"))
			{
				dtToChange.Columns["CompLogoImage"].ColumnName = "CompLogoImage2";
			}
			if (!dtToChange.Columns.Contains("CompLogoImage"))
			{
				dtToChange.Columns.Add("CompLogoImage", typeof(byte[]));
			}
			object logoValue = dtData.Rows[0][0];
			foreach (DataRow row in dtToChange.Rows)
			{
				row["CompLogoImage"] = ((logoValue != DBNull.Value) ? logoValue : null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message + " Exception from adding comp logo from dataset");
		}
	}

	public static DataTable GetVoucherHeadId(int OrganizationId, int CompanyId, int DocumentTypeId, int Id)
	{
		try
		{
			List<SqlParameter> s = new List<SqlParameter>();
			s.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId"
			});
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
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeSrNo",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_Vouchers_GetMethods", s);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<Architecture.Model.Inventory.GetItemGlIdsandItemName> GetItemListForFinancialEffects(int OrganizationId, int CompanyId)
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
				Value = "GetItemGlIdsandItemName"
			});
			return new Architecture.DAL.Inventory.GetItemGlIdsandItemName().GetData("Sp_Item_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.GetItemGlIdsandItemName> GetItemGlIdsandItemName(int OrganizationId, int CompanyId)
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
				Value = "GetItemGlIdsandItemName"
			});
			return new Architecture.DAL.Inventory.GetItemGlIdsandItemName().GetData("Sp_Item_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<GetJobLotGlIdsandName> GetJobLotGlIdsandName(int OrganizationId, int CompanyId)
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
				Value = "GetJobLotGlIdsandName"
			});
			return Conversion.ConvertDataTableToList<GetJobLotGlIdsandName>(GenericProvider.GetDataTableProc<getGlobalAllBrands>("[dbo].[SP_JobLot_ReadMethod]", p));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<Architecture.Model.Inventory.InvCropYear> getCropYearlist(int OrganizationId, int CompanyId)
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
				Value = "GetAll"
			});
			return new Architecture.DAL.Inventory.InvCropYear().GetData("Sp_InvCropYear_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<Architecture.Model.Inventory.SupplierCustomerGlAccountId> GetSupplierCustomerListForFinancialEffects(int OrganizationId, int CompanyId)
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
				Value = "GetGlAccountIdandCompanyNameBySupplierCustomerId"
			});
			return new Architecture.DAL.Inventory.SupplierCustomerGlAccountId().GetData("Sp_SupplierCustomer_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetFinancialYearId(int OrganizationId, int CompanyId)
	{
		try
		{
			List<SqlParameter> y = new List<SqlParameter>();
			y.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			y.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", y);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GLAccountDetailById(int OrganizationId, int CompanyId, int GlAccountId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@Id", GlAccountId));
			p.Add(new SqlParameter("@CoaType", "GetCompleteCoaDetailByAccountId"));
			return GenericProvider.GetDataTableProc<ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static string GetConfigurationFromAllocation(int OrganizationId, int CompanyId, string ConfigDescription)
	{
		try
		{
			string ConfigKey = "";
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
				ParameterName = "@ConfigDescription",
				Value = ConfigDescription
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetConfigurationByOrgCompandConfigDescription"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				ConfigKey = Conversion.ToString(dt.Rows[0]["ConfigKey"]);
			}
			return ConfigKey;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double AvgRateOnlyForCGS(ReportsParameters obj)
	{
		try
		{
			double AvgRate = 0.0;
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (!string.IsNullOrEmpty(obj.CropYear))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetOnlyAvgRateForCGS"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				AvgRate = Conversion.ToDouble(dt.Rows[0]["AvgRate"]);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetEqvilentByItemIdAndUomScheduleId(ReportsParameters obj)
	{
		try
		{
			double PackUom = 0.0;
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScheduleId",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetEqvilentByItemIdAndUomScheduleId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("Sp_Item_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				PackUom = Conversion.ToDouble(dt.Rows[0]["Equivalent"]);
			}
			return PackUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetVarientEquivalentByVarientId(int OrganizationId, int CompanyId, int Id)
	{
		try
		{
			double PackUom = 0.0;
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
				ParameterName = "@VarientId",
				Value = Id
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("pcc_USP_GetVarientEquivalentByVarientId", p);
			if (dt.Rows.Count > 0)
			{
				PackUom = Conversion.ToDouble(dt.Rows[0]["VarientEquivalent"]);
			}
			return PackUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetParentCategoryIdByItemCategoryId(int OrganizationId, int CompanyId, int Id)
	{
		try
		{
			int InventoryParentCategoriesId = 0;
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
				ParameterName = "@ItemCategoryId",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetParentCategoryIdByItemCategoryId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("Sp_Item_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				InventoryParentCategoriesId = Conversion.ToInt(dt.Rows[0]["InventoryParentCategoriesId"]);
			}
			return InventoryParentCategoriesId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetUomScheduleIdByItemIdAndEquivalent(ReportsParameters obj)
	{
		try
		{
			int UomschedulId = 0;
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Equivalent",
				Value = obj.RateEqvailent
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("USP_GetUomScheduleIdByItemIdAndEquivalent", p);
			if (dt.Rows.Count > 0)
			{
				UomschedulId = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return UomschedulId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherBalanceCheck(int OrganizationId, int CompanyId, int Id)
	{
		try
		{
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
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("USP_VoucherBalanceCheck", s);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetCompanyData(int OrganizationId, int CompnyId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrgCompanyTypeId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = CompnyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Company>("Sp_Company_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void RecoredRemoveByOrgCompDocAndByID(ReportsParameters obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("USP_RecoredRemoveByOrgCompDocAndByID");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.BigInt, obj.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.BigInt, obj.CompanyId);
			sqlDataBase.AddInParameter(cmd, "DocumentTypeId", SqlDbType.BigInt, obj.DocumentTypeId);
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.BigInt, obj.Id);
			sqlDataBase.AddInParameter(cmd, "UserId", SqlDbType.BigInt, obj.UserId);
			sqlDataBase.ExecuteNonQuery(cmd);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool GetERPFeaturesByCompanyId(int OrganizationId, int CompanyId, int FeatureId)
	{
		try
		{
			List<ErpFeaturesByCompany> lst = new List<ErpFeaturesByCompany>();
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
			DataTable dt = GenericProvider.GetDataTableProc<ErpFeaturesByCompany>("USP_GetERPFeaturesByCompanyId", p);
			if (dt.Rows.Count > 0)
			{
				lst = Conversion.ConvertDataTableToList<ErpFeaturesByCompany>(dt);
			}
			if (lst.Where((ErpFeaturesByCompany x) => x.Id == FeatureId).ToList().Count > 0)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGlAccountsByJobOrderId(int OrganizationId, int CompanyId, int JobOrderId)
	{
		try
		{
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
				ParameterName = "@Id",
				Value = JobOrderId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetGlAccountsByJobOrderId"
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("Sp_InvProductionJobOrder_GetAllMethod", s);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable getItemRateFromStockEvalaution(int OrganizationId, int CompanyId, int RefDocumentTypeId, int RefDocIdNo, int RefDocSubIdNo)
	{
		try
		{
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
				ParameterName = "@RefDocumentTypeId",
				Value = RefDocumentTypeId
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@RefDocIdNo",
				Value = RefDocIdNo
			});
			s.Add(new SqlParameter
			{
				ParameterName = "@RefDocSubIdNo",
				Value = RefDocSubIdNo
			});
			return GenericProvider.GetDataTableProc<VoucherHead>("usp_getItemRateFromStockEvalaution", s);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static InventoryStockEvalautionDetail FIFOImplemention(ReportsParameters obj, List<InventoryStockEvalautionDetail> lstReserveFIFO)
	{
		try
		{
			List<FIFOStockEvaluation> lstReserved = new List<FIFOStockEvaluation>();
			if (lstReserveFIFO.Count > 0)
			{
				for (int j = 0; j < lstReserveFIFO.Count; j++)
				{
					FIFOStockEvaluation objfifo = new FIFOStockEvaluation();
					objfifo.RefDocumentTypeId = lstReserveFIFO[j].RefRefDocumentTypeId;
					objfifo.RefDocIdNo = lstReserveFIFO[j].RefRefDocIdNo;
					objfifo.RefDocSubIdNo = lstReserveFIFO[j].RefRefDocSubIdNo;
					objfifo.ReserveQty = lstReserveFIFO[j].QtyOut;
					objfifo.ReserveWeight = lstReserveFIFO[j].StockWeightOut;
					lstReserved.Add(objfifo);
				}
			}
			InventoryStockEvalautionDetail mainobj = new InventoryStockEvalautionDetail();
			mainobj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUomId",
					Value = obj.stockUOM
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
				});
			}
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
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
			if (lstReserved.Count > 0)
			{
				string getXml = Conversion.ConvertListToXmlSerializer(lstReserved.ToList());
				p.Add(new SqlParameter
				{
					ParameterName = "@FIFOXML",
					Value = getXml
				});
			}
			DataTable dtstock = GenericProvider.GetDataTableProc<InventoryStockEvalautionDetail>("USP_GetStockByFifoMethod", p);
			if (dtstock != null && dtstock.Rows.Count > 0)
			{
				List<FIFOStockEvaluation> lstfifo = Conversion.ConvertDataTableToList<FIFOStockEvaluation>(dtstock);
				if (lstfifo.Count > 0)
				{
					double TotalAvailablewt = lstfifo.Sum((FIFOStockEvaluation row) => row.NetBalWeight);
					lstfifo.Sum((FIFOStockEvaluation row) => row.NetBalQty);
					double TotalRequriedWQty = obj.ItemQty;
					double TotalRequriedWeight = obj.NetWeight;
					double RateEqvailent = 0.0;
					if (obj.NetWeight <= Math.Round(TotalAvailablewt, 2))
					{
						double Required = TotalRequriedWeight;
						double RequiredQty = TotalRequriedWQty;
						double AvailableQty = 0.0;
						double ConsumedQty = 0.0;
						double Available = 0.0;
						double Consumed = 0.0;
						for (int i = 0; i < lstfifo.Count; i++)
						{
							if (lstfifo[i].AvgRate <= 0.0)
							{
								throw new Exception("Rate Not Found this Item " + obj.ItemName + " against FIFO Method");
							}
							if (lstfifo[i].RateUomId == 0)
							{
								throw new Exception("RateUomId not found  this " + obj.ItemName + " against FIFO Method");
							}
							InventoryStockEvalautionDetail objstock = new InventoryStockEvalautionDetail();
							Available = lstfifo[i].NetBalWeight;
							AvailableQty = lstfifo[i].NetBalQty;
							RateEqvailent = GetEqvilentByItemIdAndUomScheduleId(new ReportsParameters
							{
								OrganizationId = obj.OrganizationId,
								CompanyId = obj.CompanyId,
								ItemId = obj.ItemId,
								Id = lstfifo[i].RateUomId
							});
							if (RateEqvailent == 0.0)
							{
								throw new Exception("RateUom Not Found");
							}
							if (Available <= Required - Consumed)
							{
								Consumed += Available;
								ConsumedQty += AvailableQty;
								objstock.Id = lstfifo[i].Id;
								objstock.LineId = obj.LineId;
								objstock.ItemId = obj.ItemId;
								objstock.WarehouseId = obj.WarehouseId;
								objstock.RateUom = lstfifo[i].RateUomId;
								objstock.JobLotId = Conversion.ToInt(obj.JobLotId);
								objstock.InvPackingTypeId = Conversion.ToInt(obj.PackingTypeId);
								objstock.ItemUom = Conversion.ToInt(obj.stockUOM);
								objstock.CropBatch = Conversion.ToString(obj.CropYear);
								objstock.RefRefDocumentTypeId = Conversion.ToInt(lstfifo[i].RefDocumentTypeId);
								objstock.RefRefDocIdNo = Conversion.ToInt(lstfifo[i].RefDocIdNo);
								objstock.RefRefDocSubIdNo = Conversion.ToInt(lstfifo[i].RefDocSubIdNo);
								objstock.QtyOut = Conversion.ToDouble(AvailableQty);
								objstock.BillWeightOut = Conversion.ToDouble(Available);
								objstock.StockWeightOut = Conversion.ToDouble(Available);
								objstock.CgsRate = Conversion.ToDouble(lstfifo[i].AvgRate) * RateEqvailent;
								objstock.CgsAmount = objstock.BillWeightOut / RateEqvailent * objstock.CgsRate;
								mainobj.InventoryStockEvalautionDetailslist.Add(objstock);
							}
							else if (Available >= Required - Consumed)
							{
								objstock.LineId = obj.LineId;
								objstock.ItemId = obj.ItemId;
								objstock.Id = lstfifo[i].Id;
								objstock.WarehouseId = obj.WarehouseId;
								objstock.JobLotId = Conversion.ToInt(obj.JobLotId);
								objstock.InvPackingTypeId = Conversion.ToInt(obj.PackingTypeId);
								objstock.ItemUom = Conversion.ToInt(obj.stockUOM);
								objstock.CropBatch = Conversion.ToString(obj.CropYear);
								objstock.RefRefDocumentTypeId = Conversion.ToInt(lstfifo[i].RefDocumentTypeId);
								objstock.RefRefDocIdNo = Conversion.ToInt(lstfifo[i].RefDocIdNo);
								objstock.RefRefDocSubIdNo = Conversion.ToInt(lstfifo[i].RefDocSubIdNo);
								objstock.QtyOut = Conversion.ToDouble(RequiredQty - ConsumedQty);
								objstock.BillWeightOut = Conversion.ToDouble(Required - Consumed);
								objstock.StockWeightOut = Conversion.ToDouble(Required - Consumed);
								objstock.CgsRate = Conversion.ToDouble(lstfifo[i].AvgRate) * RateEqvailent;
								objstock.RateUom = lstfifo[i].RateUomId;
								objstock.CgsAmount = objstock.BillWeightOut / RateEqvailent * objstock.CgsRate;
								ConsumedQty += objstock.QtyOut;
								Consumed += objstock.BillWeightOut;
								mainobj.InventoryStockEvalautionDetailslist.Add(objstock);
							}
							if (TotalRequriedWeight == Consumed)
							{
								break;
							}
						}
						return mainobj;
					}
					throw new Exception("Weight available is " + TotalAvailablewt + " and row Weight is " + obj.NetWeight + " this item " + obj.ItemName + " against FIFO....");
				}
				throw new Exception("Stock Not Found this Item " + obj.ItemName + " against FIFO Method .....");
			}
			throw new Exception("Stock Not Found this Item " + obj.ItemName + " against FIFO Method .....");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static InventoryTransactionsPartyProcessingForFifo FIFOImplementionForPartyProcessing(ReportsParameters obj, List<InventoryTransactionsPartyProcessingForFifo> lstReserveFIFO)
	{
		try
		{
			List<FIFOPartyProcessing> lstReserved = new List<FIFOPartyProcessing>();
			if (lstReserveFIFO.Count > 0)
			{
				for (int j = 0; j < lstReserveFIFO.Count; j++)
				{
					FIFOPartyProcessing objfifo = new FIFOPartyProcessing();
					objfifo.RefDocumentTypeId = lstReserveFIFO[j].RefRefDocumentTypeId;
					objfifo.RefDocIdNo = lstReserveFIFO[j].RefRefDocIdNo;
					objfifo.RefDocSubIdNo = lstReserveFIFO[j].RefRefDocSubIdNo;
					objfifo.ReserveQty = lstReserveFIFO[j].QtyOut;
					objfifo.ReserveWeight = lstReserveFIFO[j].StockWeightOut;
					lstReserved.Add(objfifo);
				}
			}
			InventoryTransactionsPartyProcessingForFifo mainobj = new InventoryTransactionsPartyProcessingForFifo();
			mainobj.InventoryTransactionsPartyProcessingForFIFOlist = new List<InventoryTransactionsPartyProcessingForFifo>();
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@StockPartyId",
				Value = obj.StockPartyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUomId",
					Value = obj.stockUOM
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
			if (obj.PackingTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackingTypeId",
					Value = obj.PackingTypeId
				});
			}
			if (obj.CropYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
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
			if (lstReserved.Count > 0)
			{
				string getXml = Conversion.ConvertListToXmlSerializer(lstReserved.ToList());
				p.Add(new SqlParameter
				{
					ParameterName = "@FIFOXML",
					Value = getXml
				});
			}
			DataTable dtstock = GenericProvider.GetDataTableProc<InventoryTransactionsPartyProcessingForFifo>("USP_GetStockByFifoMethodPartyProcessing", p);
			if (dtstock != null && dtstock.Rows.Count > 0)
			{
				List<FIFOPartyProcessing> lstfifo = Conversion.ConvertDataTableToList<FIFOPartyProcessing>(dtstock);
				if (lstfifo.Count > 0)
				{
					double TotalAvailablewt = lstfifo.Sum((FIFOPartyProcessing row) => row.NetBalWeight);
					lstfifo.Sum((FIFOPartyProcessing row) => row.NetBalQty);
					double TotalRequriedWQty = obj.ItemQty;
					double TotalRequriedWeight = obj.NetWeight;
					if (obj.NetWeight <= Math.Round(TotalAvailablewt, 2))
					{
						double Required = TotalRequriedWeight;
						double RequiredQty = TotalRequriedWQty;
						double AvailableQty = 0.0;
						double ConsumedQty = 0.0;
						double Available = 0.0;
						double Consumed = 0.0;
						for (int i = 0; i < lstfifo.Count; i++)
						{
							InventoryTransactionsPartyProcessingForFifo objstock = new InventoryTransactionsPartyProcessingForFifo();
							Available = lstfifo[i].NetBalWeight;
							AvailableQty = lstfifo[i].NetBalQty;
							if (Available <= Required - Consumed)
							{
								Consumed += Available;
								ConsumedQty += AvailableQty;
								objstock.Id = lstfifo[i].Id;
								objstock.LineId = obj.LineId;
								objstock.ItemId = obj.ItemId;
								objstock.WarehouseId = obj.WarehouseId;
								objstock.JobLotId = Conversion.ToInt(obj.JobLotId);
								objstock.InvPackingTypeId = Conversion.ToInt(obj.PackingTypeId);
								objstock.ItemUom = Conversion.ToInt(obj.stockUOM);
								objstock.CropYearId = Conversion.ToInt(obj.CropYearId);
								objstock.RefRefDocumentTypeId = Conversion.ToInt(lstfifo[i].RefDocumentTypeId);
								objstock.RefRefDocIdNo = Conversion.ToInt(lstfifo[i].RefDocIdNo);
								objstock.RefRefDocSubIdNo = Conversion.ToInt(lstfifo[i].RefDocSubIdNo);
								objstock.QtyOut = Conversion.ToDouble(AvailableQty);
								objstock.BillWeightOut = Conversion.ToDouble(Available);
								objstock.StockWeightOut = Conversion.ToDouble(Available);
								mainobj.InventoryTransactionsPartyProcessingForFIFOlist.Add(objstock);
							}
							else if (Available >= Required - Consumed)
							{
								objstock.LineId = obj.LineId;
								objstock.ItemId = obj.ItemId;
								objstock.Id = lstfifo[i].Id;
								objstock.WarehouseId = obj.WarehouseId;
								objstock.JobLotId = Conversion.ToInt(obj.JobLotId);
								objstock.InvPackingTypeId = Conversion.ToInt(obj.PackingTypeId);
								objstock.ItemUom = Conversion.ToInt(obj.stockUOM);
								objstock.CropYearId = Conversion.ToInt(obj.CropYearId);
								objstock.RefRefDocumentTypeId = Conversion.ToInt(lstfifo[i].RefDocumentTypeId);
								objstock.RefRefDocIdNo = Conversion.ToInt(lstfifo[i].RefDocIdNo);
								objstock.RefRefDocSubIdNo = Conversion.ToInt(lstfifo[i].RefDocSubIdNo);
								objstock.QtyOut = Conversion.ToDouble(RequiredQty - ConsumedQty);
								objstock.BillWeightOut = Conversion.ToDouble(Required - Consumed);
								objstock.StockWeightOut = Conversion.ToDouble(Required - Consumed);
								ConsumedQty += objstock.QtyOut;
								Consumed += objstock.BillWeightOut;
								mainobj.InventoryTransactionsPartyProcessingForFIFOlist.Add(objstock);
							}
							if (TotalRequriedWeight == Consumed)
							{
								break;
							}
						}
						return mainobj;
					}
					throw new Exception("Weight available is " + TotalAvailablewt + " and row Weight is " + obj.NetWeight + " this item " + obj.ItemName + " against FIFO....");
				}
				throw new Exception("Stock Not Found this Item " + obj.ItemName + " against FIFO Method .....");
			}
			throw new Exception("Stock Not Found this Item " + obj.ItemName + " against FIFO Method .....");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static InventoryStockEvalautionDetail FIFOImplementionForConcrete(ReportsParameters obj, List<InventoryStockEvalautionDetail> lstReserveFIFO)
	{
		try
		{
			List<FIFOStockEvaluation> lstReserved = new List<FIFOStockEvaluation>();
			if (lstReserveFIFO.Count > 0)
			{
				for (int j = 0; j < lstReserveFIFO.Count; j++)
				{
					FIFOStockEvaluation objfifo = new FIFOStockEvaluation();
					objfifo.RefDocumentTypeId = lstReserveFIFO[j].RefRefDocumentTypeId;
					objfifo.RefDocIdNo = lstReserveFIFO[j].RefRefDocIdNo;
					objfifo.RefDocSubIdNo = lstReserveFIFO[j].RefRefDocSubIdNo;
					objfifo.ReserveQty = lstReserveFIFO[j].QtyOut;
					objfifo.ReserveWeight = lstReserveFIFO[j].StockWeightOut;
					lstReserved.Add(objfifo);
				}
			}
			InventoryStockEvalautionDetail mainobj = new InventoryStockEvalautionDetail();
			mainobj.InventoryStockEvalautionDetailslist = new List<InventoryStockEvalautionDetail>();
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VarientId",
					Value = obj.stockUOM
				});
			}
			if (obj.WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = obj.WarehouseId
				});
			}
			if (obj.JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = obj.JobLotId
				});
			}
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
			if (lstReserved.Count > 0)
			{
				string getXml = Conversion.ConvertListToXmlSerializer(lstReserved.ToList());
				p.Add(new SqlParameter
				{
					ParameterName = "@FIFOXML",
					Value = getXml
				});
			}
			DataTable dtstock = GenericProvider.GetDataTableProc<InventoryStockEvalautionDetail>("[pcc].[USP_GetStockByFifoMethod]", p);
			if (dtstock != null && dtstock.Rows.Count > 0)
			{
				List<FIFOStockEvaluation> lstfifo = Conversion.ConvertDataTableToList<FIFOStockEvaluation>(dtstock);
				if (lstfifo.Count > 0)
				{
					lstfifo.Sum((FIFOStockEvaluation row) => row.NetBalWeight);
					double TotalAvailableQty = lstfifo.Sum((FIFOStockEvaluation row) => row.NetBalQty);
					double TotalRequriedWQty = obj.ItemQty;
					double TotalRequriedWeight = obj.NetWeight;
					if (obj.ItemQty <= TotalAvailableQty)
					{
						double RequiredQty = TotalRequriedWQty;
						double AvailableQty = 0.0;
						double ConsumedQty = 0.0;
						double Available = 0.0;
						double Consumed = 0.0;
						for (int i = 0; i < lstfifo.Count; i++)
						{
							if (lstfifo[i].AvgRate <= 0.0)
							{
								throw new Exception("Rate Not Found this Item " + obj.ItemName + " against FIFO Method");
							}
							InventoryStockEvalautionDetail objstock = new InventoryStockEvalautionDetail();
							Available = lstfifo[i].NetBalWeight;
							AvailableQty = lstfifo[i].NetBalQty;
							if (AvailableQty <= RequiredQty - ConsumedQty)
							{
								Consumed += Available;
								ConsumedQty += AvailableQty;
								objstock.Id = lstfifo[i].Id;
								objstock.LineId = obj.LineId;
								objstock.ItemId = obj.ItemId;
								objstock.WarehouseId = obj.WarehouseId;
								objstock.RateUom = lstfifo[i].RateUomId;
								objstock.JobLotId = Conversion.ToInt(obj.JobLotId);
								objstock.InvPackingTypeId = Conversion.ToInt(obj.PackingTypeId);
								objstock.ItemUom = Conversion.ToInt(obj.stockUOM);
								objstock.VarientId = Conversion.ToInt(obj.stockUOM);
								objstock.CropBatch = Conversion.ToString(obj.CropYear);
								objstock.RefRefDocumentTypeId = Conversion.ToInt(lstfifo[i].RefDocumentTypeId);
								objstock.RefRefDocIdNo = Conversion.ToInt(lstfifo[i].RefDocIdNo);
								objstock.RefRefDocSubIdNo = Conversion.ToInt(lstfifo[i].RefDocSubIdNo);
								objstock.QtyOut = Conversion.ToDouble(AvailableQty);
								objstock.BillWeightOut = objstock.QtyOut;
								objstock.StockWeightOut = objstock.QtyOut;
								objstock.CgsRate = Conversion.ToDouble(lstfifo[i].AvgRate);
								objstock.CgsAmount = objstock.QtyOut * objstock.CgsRate;
								mainobj.InventoryStockEvalautionDetailslist.Add(objstock);
							}
							else if (AvailableQty >= RequiredQty - ConsumedQty)
							{
								objstock.LineId = obj.LineId;
								objstock.ItemId = obj.ItemId;
								objstock.Id = lstfifo[i].Id;
								objstock.WarehouseId = obj.WarehouseId;
								objstock.JobLotId = Conversion.ToInt(obj.JobLotId);
								objstock.InvPackingTypeId = Conversion.ToInt(obj.PackingTypeId);
								objstock.ItemUom = Conversion.ToInt(obj.stockUOM);
								objstock.VarientId = Conversion.ToInt(obj.stockUOM);
								objstock.CropBatch = Conversion.ToString(obj.CropYear);
								objstock.RefRefDocumentTypeId = Conversion.ToInt(lstfifo[i].RefDocumentTypeId);
								objstock.RefRefDocIdNo = Conversion.ToInt(lstfifo[i].RefDocIdNo);
								objstock.RefRefDocSubIdNo = Conversion.ToInt(lstfifo[i].RefDocSubIdNo);
								objstock.QtyOut = Conversion.ToDouble(RequiredQty - ConsumedQty);
								objstock.BillWeightOut = objstock.QtyOut;
								objstock.StockWeightOut = objstock.QtyOut;
								objstock.CgsRate = Conversion.ToDouble(lstfifo[i].AvgRate);
								objstock.RateUom = lstfifo[i].RateUomId;
								objstock.CgsAmount = objstock.QtyOut * objstock.CgsRate;
								ConsumedQty += objstock.QtyOut;
								Consumed += objstock.BillWeightOut;
								mainobj.InventoryStockEvalautionDetailslist.Add(objstock);
							}
							if (TotalRequriedWQty == ConsumedQty)
							{
								break;
							}
						}
						return mainobj;
					}
					throw new Exception("Qty available is " + TotalAvailableQty + " and row Qty is " + obj.ItemQty + " this item " + obj.ItemName + " against FIFO....");
				}
				throw new Exception("Stock Not Found this Item " + obj.ItemName + " against FIFO Method .....");
			}
			throw new Exception("Stock Not Found this Item " + obj.ItemName + " against FIFO Method .....");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static List<InventoryStockEvalautionDetail> GetInventoryStockEvalautionDetailByOtherIds(ReportsParameters obj)
	{
		try
		{
			new List<InventoryStockEvalautionDetail>();
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
				ParameterName = "@OtherDocumentTypeId",
				Value = obj.OtherDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherDocNoId",
				Value = obj.OtherDocNoId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OtherSubDocNoId",
				Value = obj.OtherSubDocNoId
			});
			return Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail>(GenericProvider.GetDataTableProc<Architecture.Model.Company>("USP_GetInventoryStockEvalautionDetailByOtherIds", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<InventoryStockEvalautionDetail> GetInventoryStockEvalautionDetailByOtherIdsAndStockIdsForExportInoice(ReportsParameters obj)
	{
		try
		{
			new List<InventoryStockEvalautionDetail>();
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
				ParameterName = "@OtherDocumentTypeId",
				Value = obj.OtherDocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@InvoiceId",
				Value = obj.ExImInvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@InvoiceDetailId",
				Value = obj.InvoiceDetailId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemUomId",
				Value = obj.ItemUomId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobLotId",
				Value = obj.JobLotId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@PackingTypeId",
				Value = obj.PackingTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@WarehouseId",
				Value = obj.WarehouseId
			});
			if (obj.CropYearId > 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYearId",
					Value = obj.CropYearId
				});
			}
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			return Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail>(GenericProvider.GetDataTableProc<Architecture.Model.Company>("USP_GetInventoryStockEvalautionDetailByOtherIdsAndStockIdsForExportInoice", p));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SearchByObject(Architecture.Model.Inventory.UOMSchedule obj)
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
				ParameterName = "@ItemId",
				Value = obj.ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByItemID"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.UOMSchedule>("Sp_UOMSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<DMSAttachments> CommonMethodForAttachmentsToPhysicallySave(ServerCredentials Sc, bool IsVpsAttachmentsServiceOn, string AttachmentFolderPath, List<DMSAttachments> AttachmentsList, List<string> copiedFiles)
	{
		try
		{
			foreach (DMSAttachments item in AttachmentsList)
			{
				double FileSize = 0.0;
				string FileFullPath = "";
				if (item.FileCustomName != null && item.FileCustomName != string.Empty)
				{
					item.UploadedFileCustomName = item.FileCustomName;
				}
				if ((item.UploadedFileCustomName != null && item.UploadedFileCustomName != string.Empty) || item.Id > 1)
				{
					item.Attachment = item.Attachment;
					item.UploadedFileCustomName = item.UploadedFileCustomName;
					continue;
				}
				if (!IsVpsAttachmentsServiceOn)
				{
					FileFullPath = item.Attachment;
					if (!File.Exists(Path.Combine(AttachmentFolderPath, Path.GetFileName(item.Attachment))))
					{
						item.Attachment = Path.GetFileName(FileFullPath);
						string baseName = FileFullPath.Split('?')[0];
						string extension = ((baseName.Split('.').Length > 1) ? baseName.Split('.').Last() : string.Empty);
						string CustomFullName = GenerateCustomFileName() + "." + extension;
						item.UploadedFileCustomName = CustomFullName;
						FileInfo info = new FileInfo(FileFullPath);
						if (info.Exists)
						{
							FileSize = (double)info.Length / 1048576.0;
						}
						if (FileSize > 5.0)
						{
							throw new Exception("File Size Exceeds 5MB Of File: " + item.Attachment);
						}
						string SavePath = Path.Combine(AttachmentFolderPath, item.UploadedFileCustomName);
						if (!File.Exists(SavePath))
						{
							File.Copy(FileFullPath, SavePath, overwrite: true);
							copiedFiles.Add(SavePath);
						}
					}
					else
					{
						item.Attachment = Path.GetFileName(item.Attachment);
						item.UploadedFileCustomName = "";
					}
					continue;
				}
				bool FileExist = false;
				string uri = "ftp://" + Sc.ServerIp + "/" + Path.GetFileName(item.Attachment);
				try
				{
					FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
					obj.Method = "MDTM";
					obj.UsePassive = true;
					obj.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
					using ((FtpWebResponse)obj.GetResponse())
					{
						FileExist = true;
					}
				}
				catch (WebException ex2)
				{
					if (ex2.Response is FtpWebResponse { StatusCode: not FtpStatusCode.ActionNotTakenFileUnavailable })
					{
						throw new Exception(ex2.Message);
					}
				}
				if (!FileExist)
				{
					FileFullPath = item.Attachment;
					item.Attachment = Path.GetFileName(FileFullPath);
					string baseName2 = FileFullPath.Split('?')[0];
					string extension2 = ((baseName2.Split('.').Length > 1) ? baseName2.Split('.').Last() : string.Empty);
					string CustomFullName2 = GenerateCustomFileName() + "." + extension2;
					item.UploadedFileCustomName = CustomFullName2;
					FileInfo info2 = new FileInfo(FileFullPath);
					if (info2.Exists)
					{
						FileSize = (double)info2.Length / 1048576.0;
					}
					if (FileSize > 5.0)
					{
						throw new Exception("File Size Exceeds 5MB Of File: " + item.Attachment);
					}
					string url = "ftp://" + Sc.ServerIp + "/" + item.UploadedFileCustomName;
					try
					{
						FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
						request.Method = "STOR";
						request.UsePassive = true;
						request.UseBinary = true;
						request.KeepAlive = false;
						request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						byte[] fileContents;
						using (FileStream fs = info2.OpenRead())
						{
							fileContents = new byte[info2.Length];
							fs.Read(fileContents, 0, fileContents.Length);
						}
						request.ContentLength = fileContents.Length;
						using (Stream requestStream = request.GetRequestStream())
						{
							requestStream.Write(fileContents, 0, fileContents.Length);
						}
						using ((FtpWebResponse)request.GetResponse())
						{
							copiedFiles.Add(item.UploadedFileCustomName);
						}
					}
					catch (WebException webEx)
					{
						if (webEx.Response is FtpWebResponse response)
						{
							throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
						}
						throw new Exception("Error: " + webEx.Message);
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
				}
				else
				{
					item.Attachment = Path.GetFileName(item.Attachment);
					item.UploadedFileCustomName = "";
				}
			}
			return AttachmentsList;
		}
		catch (Exception ex3)
		{
			throw new Exception(ex3.Message);
		}
	}

	public static void DeleteAttachmentsPhysically(ServerCredentials Sc, bool IsVpsAttachmentsServiceOn, string AttachmentFolderPath, List<DMSAttachments> DeleteAttachmentsList)
	{
		try
		{
			foreach (DMSAttachments item in DeleteAttachmentsList)
			{
				if (!IsVpsAttachmentsServiceOn)
				{
					string Path = AttachmentFolderPath + "\\" + item.Attachment;
					if (!File.Exists(Path))
					{
						continue;
					}
					using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite))
					{
						fs.Flush();
						fs.Close();
						fs.Dispose();
					}
					bool fileDeleted = false;
					int attempts = 0;
					while (!fileDeleted && attempts < 5)
					{
						try
						{
							File.Delete(Path);
							fileDeleted = true;
						}
						catch (IOException)
						{
							attempts++;
							Thread.Sleep(500);
						}
					}
					continue;
				}
				string uri = "ftp://" + Sc.ServerIp + "/" + item.Attachment;
				try
				{
					FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
					obj.Method = "DELE";
					obj.UsePassive = true;
					obj.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
					using FtpWebResponse deleteResponse = (FtpWebResponse)obj.GetResponse();
					if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
					{
						throw new Exception("Failed to delete file from VPS.");
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Error while deleting file from VPS: " + ex.Message);
				}
			}
		}
		catch (Exception ex3)
		{
			throw new Exception(ex3.Message);
		}
	}

	public static void DeleteAttachmentsPhysicallyInException(ServerCredentials Sc, bool IsVpsAttachmentsServiceOn, string AttachmentFolderPath, List<string> CopiedFiles)
	{
		try
		{
			foreach (string file in CopiedFiles)
			{
				if (!IsVpsAttachmentsServiceOn)
				{
					using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
					{
						fs.Flush();
						fs.Close();
						fs.Dispose();
					}
					bool fileDeleted = false;
					int attempts = 0;
					while (!fileDeleted && attempts < 5)
					{
						try
						{
							File.Delete(file);
							fileDeleted = true;
						}
						catch (IOException)
						{
							attempts++;
							Thread.Sleep(500);
						}
					}
					continue;
				}
				string uri = "ftp://" + Sc.ServerIp + "/" + file;
				try
				{
					FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
					obj.Method = "DELE";
					obj.UsePassive = true;
					obj.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
					using FtpWebResponse deleteResponse = (FtpWebResponse)obj.GetResponse();
					if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
					{
						throw new Exception("Failed to delete file from VPS.");
					}
				}
				catch (Exception)
				{
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static ServerCredentials GetServerCredentials()
	{
		ServerCredentials obj = new ServerCredentials();
		try
		{
			string ServerIp = ConfigurationManager.ConnectionStrings["VPSName"].ConnectionString;
			string VPSUser = ConfigurationManager.ConnectionStrings["VPSUser"].ConnectionString;
			string VPSPwd = ConfigurationManager.ConnectionStrings["VPSPwd"].ConnectionString;
			if (!string.IsNullOrEmpty(ServerIp))
			{
				obj.ServerIp = Encryption.DecryptString(ServerIp);
				if (!string.IsNullOrEmpty(VPSUser))
				{
					obj.UserName = Encryption.DecryptString(VPSUser);
					if (!string.IsNullOrEmpty(VPSPwd))
					{
						obj.Password = Encryption.DecryptString(VPSPwd);
						return obj;
					}
					throw new InvalidOperationException("The VPS password configuration is not set.");
				}
				throw new InvalidOperationException("The VPS user configuration is not set.");
			}
			throw new InvalidOperationException("The server IP configuration is not set.");
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Error while getting server credentials", ex);
		}
	}

	private OperationResult UploadFileToFtp(string filePath)
	{
		ServerCredentials credentials = GetServerCredentials();
		OperationResult result = new OperationResult();
		if (string.IsNullOrEmpty(filePath))
		{
			result.Status = false;
			result.Message = "Please select a file to upload.";
			return result;
		}
		FileInfo fileInfo = new FileInfo(filePath);
		if (!fileInfo.Exists)
		{
			result.Status = false;
			result.Message = "Selected file does not exist.";
			return result;
		}
		string uri = "ftp://" + credentials.ServerIp + "/" + fileInfo.Name;
		try
		{
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
			request.Method = "STOR";
			request.UsePassive = true;
			request.UseBinary = true;
			request.KeepAlive = false;
			request.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
			byte[] fileContents;
			using (FileStream fs = fileInfo.OpenRead())
			{
				fileContents = new byte[fileInfo.Length];
				fs.Read(fileContents, 0, fileContents.Length);
			}
			request.ContentLength = fileContents.Length;
			using (Stream requestStream = request.GetRequestStream())
			{
				requestStream.Write(fileContents, 0, fileContents.Length);
			}
			using FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
			result.Status = true;
			result.Message = "Upload Complete: " + ftpWebResponse.StatusDescription;
		}
		catch (WebException webEx)
		{
			if (webEx.Response is FtpWebResponse response)
			{
				result.Status = false;
				result.Message = "FTP Error: " + response.StatusDescription + " - " + webEx.Message;
			}
			else
			{
				result.Status = false;
				result.Message = "Error: " + webEx.Message;
			}
		}
		catch (Exception ex)
		{
			result.Status = false;
			result.Message = "Error: " + ex.Message;
		}
		return result;
	}

	private OperationResult DownloadFileFromFtp(string fileName)
	{
		ServerCredentials credentials = GetServerCredentials();
		OperationResult result = new OperationResult();
		if (string.IsNullOrEmpty(fileName))
		{
			result.Status = false;
			result.Message = "Please provide the file name to download.";
			return result;
		}
		using FolderBrowserDialog folderDialog = new FolderBrowserDialog();
		if (folderDialog.ShowDialog() == DialogResult.OK)
		{
			string localSavePath = folderDialog.SelectedPath;
			string uri = "ftp://" + credentials.ServerIp + "/" + fileName;
			try
			{
				FtpWebRequest obj = (FtpWebRequest)WebRequest.Create(uri);
				obj.Method = "RETR";
				obj.UsePassive = true;
				obj.UseBinary = true;
				obj.KeepAlive = false;
				obj.Credentials = new NetworkCredential(credentials.UserName, credentials.Password);
				using FtpWebResponse ftpWebResponse = (FtpWebResponse)obj.GetResponse();
				using Stream responseStream = ftpWebResponse.GetResponseStream();
				using (FileStream fileStream = new FileStream(Path.Combine(localSavePath, fileName), FileMode.Create))
				{
					responseStream.CopyTo(fileStream);
				}
				result.Status = true;
				result.Message = "Download Completed.";
				return result;
			}
			catch (WebException webEx)
			{
				if (webEx.Response is FtpWebResponse response)
				{
					result.Status = false;
					result.Message = "FTP Error: " + response.StatusDescription + " - " + webEx.Message;
					return result;
				}
				result.Status = false;
				result.Message = "Error: " + webEx.Message;
				return result;
			}
			catch (Exception ex)
			{
				result.Status = false;
				result.Message = "Error: " + ex.Message;
				return result;
			}
		}
		result.Status = false;
		result.Message = "Download cancelled.";
		return result;
	}

	public static string GenerateCustomFileName()
	{
		DateTime now = DateTime.Now;
		string text = $"{clsGlobalVariables.UserAccount.OrganizationId}_{clsGlobalVariables.UserAccount.CompanyId}_{now:yyyy_MM_dd_HHmmssfff}";
		string guid = Guid.NewGuid().ToString("N");
		return text + "_" + guid;
	}

	private static int GetRandomInt(int min, int max, Random random)
	{
		return random.Next(min, max + 1);
	}

	public static DataTable SelectAndOptionallyRenameColumns(DataTable sourceTable, Dictionary<string, string> columnMappings)
	{
		try
		{
			if (columnMappings == null || columnMappings.Count == 0)
			{
				throw new ArgumentException("columnMappings must contain at least one entry.");
			}
			DataTable result = new DataTable();
			foreach (KeyValuePair<string, string> mapping in columnMappings)
			{
				string originalName = mapping.Key;
				string newName = (string.IsNullOrWhiteSpace(mapping.Value) ? originalName : mapping.Value);
				if (!sourceTable.Columns.Contains(originalName))
				{
					throw new ArgumentException("Column '" + originalName + "' does not exist in the source table.");
				}
				result.Columns.Add(newName, sourceTable.Columns[originalName].DataType);
			}
			foreach (DataRow row in sourceTable.Rows)
			{
				object[] values = columnMappings.Select(delegate(KeyValuePair<string, string> m)
				{
					object obj = row[m.Key];
					return (obj is string text) ? text.Replace("\r", " ").Replace("\n", " ").Trim() : obj;
				}).ToArray();
				result.Rows.Add(values);
			}
			return result;
		}
		catch (Exception ex)
		{
			throw new Exception("Error selecting and optionally renaming columns: " + ex.Message, ex);
		}
	}
}
