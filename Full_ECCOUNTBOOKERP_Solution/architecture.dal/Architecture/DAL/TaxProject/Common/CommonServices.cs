using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.DAL.TaxProject.Common;

public class CommonServices : DALBase
{
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
			DataTable dt = GenericProvider.GetDataTableProcForServer<ConfigrationsAllocation>("Sp_ConfigrationsAllocation_GetAllMethod", p);
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

	public static DataTable GLAccountDetailById(int OrganizationId, int CompanyId, int GlAccountId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationId", OrganizationId));
			p.Add(new SqlParameter("@CompanyId", CompanyId));
			p.Add(new SqlParameter("@Id", GlAccountId));
			p.Add(new SqlParameter("@CoaType", "GetCompleteCoaDetailByAccountId"));
			return GenericProvider.GetDataTableProcForServer<ChartofAccount>("Sp_ChartofAccount_GetAllMethodFromCOA", p);
		}
		catch (Exception ex)
		{
			throw ex;
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
			DataTable dt = GenericProvider.GetDataTableProcForServer<FIFOStockEvaluation>("Sp_Item_GetAllMethod", p);
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

	public static List<GetItemGlIdsandItemName> GetItemGlIdsandItemName(int OrganizationId, int CompanyId)
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
			return Conversion.ConvertDataTableToList<GetItemGlIdsandItemName>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("Sp_Item_GetAllMethod", p));
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
			return Conversion.ConvertDataTableToList<GetJobLotGlIdsandName>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("SP_JobLot_ReadMethod", p));
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
			return Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail>(GenericProvider.GetDataTableProcForServer<Architecture.Model.Company>("USP_GetInventoryStockEvalautionDetailByOtherIds", p));
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
			DataTable dt = GenericProvider.GetDataTableProcForServer<ErpFeaturesByCompany>("USP_GetERPFeaturesByCompanyId", p);
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
			return GenericProvider.GetDataTableProcForServer<VoucherHead>("Sp_Vouchers_GetMethods", s);
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
			DataTable dtstock = GenericProvider.GetDataTableProcForServer<InventoryStockEvalautionDetail>("USP_GetStockByFifoMethod", p);
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
}
