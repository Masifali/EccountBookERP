using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;

namespace Architecture.BLL;

public class GetAvgRatesAndStockInHand
{
	public static DataTable GetBalanceQtyAndWeightFromStockEvaluation(ReportsParameters obj)
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
				ParameterName = "@DateTo",
				Value = obj.DocDate
			});
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
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
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
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.stockUOM
				});
			}
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_GetBalanceQtyAndWeightFromStockEvaluation", p);
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
					ParameterName = "@RecId",
					Value = obj.Id
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

	public static DataTable GetAvgRateQtyAndStockInHand(ReportsParameters obj)
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
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
			}
			if (obj.ItemConditionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemConditionId",
					Value = obj.ItemConditionId
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
			if (obj.StoreRackId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RackId",
					Value = obj.StoreRackId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvgRateQtyAndStockInHand"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockInHandAndAvgRateFromEvaluation(ReportsParameters obj)
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
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvgRateQtyAndStockInHand"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockInHandAndAvgRateFromEvaluationConcrete(int OrganizationId, int CompanyId, int ItemId, DateTime DocDate, int WarehouseId = 0, int JobLotId = 0, int ItemUomId = 0)
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
				ParameterName = "@ItemId",
				Value = ItemId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = DocDate
			});
			if (WarehouseId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@WarehouseId",
					Value = WarehouseId
				});
			}
			if (JobLotId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobLotId",
					Value = JobLotId
				});
			}
			if (ItemUomId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = ItemUomId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetStockInHandAndAvgRateFromEvaluation"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetQtyInHandForPackingMaterialAndStoreItems(ReportsParameters obj)
	{
		try
		{
			double QtyInHand = 0.0;
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
			p.Add(new SqlParameter
			{
				ParameterName = "@WarehouseId",
				Value = obj.WarehouseId
			});
			if (obj.ItemConditionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemConditionId",
					Value = obj.ItemConditionId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetQtyInHandForPackingMaterialAndStoreItems"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				QtyInHand = Conversion.ToDouble(dt.Rows[0][0]);
			}
			return QtyInHand;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetStockInHandFromInventoryTrasactions(ReportsParameters obj)
	{
		try
		{
			double StockInHand = 0.0;
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
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
				});
			}
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemUomId",
					Value = obj.stockUOM
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetStockInHandFromInventoryTrasactions"
			});
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				StockInHand = Conversion.ToDouble(dt.Rows[0][0]);
			}
			return StockInHand;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBalQtyAndWeightForPartyProcessing(ReportsParameters obj)
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
				ParameterName = "@StockPartyId",
				Value = obj.StockPartyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@WarehouseId",
				Value = obj.WarehouseId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetBalQtyAndWeightForPartyProcessing"
			});
			return GenericProvider.GetDataTableProc<ReportsParameters>("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAvailableStock(ReportsParameters obj)
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
				ParameterName = "@DateTo",
				Value = obj.DocDate
			});
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
			if (obj.CropYear != null && obj.CropYear != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CropYear",
					Value = obj.CropYear
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
			if (obj.ItemUomId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUomId",
					Value = obj.ItemUomId
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("usp_getAvailableStock", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static StockResult GetAvailableStockByTransactionOrEvaluation(ReportsParameters obj)
	{
		try
		{
			StockResult result = new StockResult();
			using (SqlConnection con = ConnectionObject.SQL())
			{
				using SqlCommand cmd = new SqlCommand("usp_getAvailableStock", con);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@OrganizationId", obj.OrganizationId);
				cmd.Parameters.AddWithValue("@CompanyId", obj.CompanyId);
				cmd.Parameters.AddWithValue("@ItemId", obj.ItemId);
				cmd.Parameters.AddWithValue("@DateTo", obj.DocDate);
				if (obj.WarehouseId != 0)
				{
					cmd.Parameters.AddWithValue("@WarehouseId", obj.WarehouseId);
				}
				if (obj.JobLotId != 0)
				{
					cmd.Parameters.AddWithValue("@JobLotId", obj.JobLotId);
				}
				if (!string.IsNullOrEmpty(obj.CropYear))
				{
					cmd.Parameters.AddWithValue("@CropYear", obj.CropYear);
				}
				if (obj.PackingTypeId != 0)
				{
					cmd.Parameters.AddWithValue("@PackingTypeId", obj.PackingTypeId);
				}
				if (obj.ItemUomId != 0)
				{
					cmd.Parameters.AddWithValue("@PackUomId", obj.ItemUomId);
				}
				if (obj.WithoutStockLogic != 0)
				{
					cmd.Parameters.AddWithValue("@WithouStockLogic", obj.WithoutStockLogic);
				}
				if (obj.ActionId != 0)
				{
					cmd.Parameters.AddWithValue("@ActionId", obj.ActionId);
				}
				con.Open();
				using SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					result.BalQty = ((reader["BalQty"] != DBNull.Value) ? Convert.ToDecimal(reader["BalQty"]) : 0m);
					result.BalWeight = ((reader["BalWeight"] != DBNull.Value) ? Convert.ToDecimal(reader["BalWeight"]) : 0m);
				}
			}
			return result;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockByFifoMethod(ReportsParameters obj)
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
			if (obj.stockUOM != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PackUomId",
					Value = obj.stockUOM
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("USP_GetStockByFifoMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStockByFifoMethodConcrete(ReportsParameters obj)
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
			return GenericProvider.GetDataTableProc<InventoryStockEvalautionDetail>("[pcc].[USP_GetStockByFifoMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double getAvgRateByItemId(ReportsParameters obj)
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
					ParameterName = "@RecId",
					Value = obj.Id
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
			DataTable dt = GenericProvider.GetDataTableProc<ReportsParameters>("fed.usp_getAvgRateByItemId", p);
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
}
