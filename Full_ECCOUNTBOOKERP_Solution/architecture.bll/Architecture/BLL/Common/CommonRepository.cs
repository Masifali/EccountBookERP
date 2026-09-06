using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Inventory;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using QRCoder;

namespace Architecture.BLL.Common;

public static class CommonRepository
{
	public static ReportDocument cryRpt = new ReportDocument();

	public static DataTable GetDatabaseTable()
	{
		DataTable dt = null;
		try
		{
			return CommonProvider.GetDataTable("usp_Sys_GetTable");
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetDataTable(string procedureName, List<SqlParameter> p = null)
	{
		DataTable dt = null;
		try
		{
			return CommonProvider.GetDataTable(procedureName, p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataSet GetDataSet(string procedureName, List<SqlParameter> p = null)
	{
		DataSet ds = null;
		try
		{
			return CommonProvider.GetDataSet(procedureName, p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetTableProperty(string tblName)
	{
		DataTable dt = null;
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@tblName",
				Value = tblName
			});
			return CommonProvider.GetDataTable("usp_Sys_GetProperty", p);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static bool GenerateSetScript(string tblName)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@tblName",
				Value = tblName
			});
			return CommonProvider.ExecuteProcedure("usp_Sys_GenerateSetScript", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static bool GenerateGetScript(string tblName)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@tblName",
				Value = tblName
			});
			return CommonProvider.ExecuteProcedure("usp_Sys_GenerateGetScript", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static bool GenerateTrigger(string tblName)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@tblName",
				Value = tblName
			});
			return CommonProvider.ExecuteProcedure("usp_Sys_GenerateTrigger", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void CrystalReportConvertinPdf(DataTable ReportDataTable, string Reportname)
	{
		string reportPath = string.Empty;
		string Usr = clsGlobalVariables.UserAccount.ID.ToString();
		string firstName = clsGlobalVariables.UserAccount.FirstName;
		string lstName = clsGlobalVariables.UserAccount.LastName;
		string UserName = firstName + lstName;
		try
		{
			List<SqlParameter> lst = new List<SqlParameter>();
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Report Folder Path").ToList();
			if (list.Count > 0)
			{
				reportPath = list[0].ConfigKey.ToString() + "\\";
				lst.Add(new SqlParameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress));
				lst.Add(new SqlParameter("@CompanyName", clsGlobalVariables.UserAccount.CompName));
			}
			reportPath += Reportname;
			if (File.Exists(reportPath))
			{
				ResetObject();
				cryRpt = new ReportDocument();
				cryRpt.Load(reportPath);
				cryRpt.SetDataSource(ReportDataTable);
				foreach (SqlParameter items in lst)
				{
					cryRpt.SetParameterValue(items.ParameterName, items.Value);
				}
				string pdffilepath = "";
				List<ConfigrationsAllocation> pathfile = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
				if (pathfile.Count > 0)
				{
					pdffilepath = pathfile[0].ConfigKey.ToString() + "\\";
				}
				cryRpt.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
				cryRpt.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
				DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
				objDiskOpt.DiskFileName = pdffilepath + Reportname + Usr + UserName + ".pdf";
				cryRpt.ExportOptions.DestinationOptions = objDiskOpt;
				cryRpt.Export();
				MessageBox.Show("Exported Successful");
				ResetObject();
				return;
			}
			throw new Exception("Report Configuration not found");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void ResetObject()
	{
		cryRpt.Dispose();
		cryRpt.Close();
	}

	public static bool ExecuteStorProcedure(string ProcedureName = null, List<SqlParameter> p = null)
	{
		return CommonProvider.ExecuteProcedure(ProcedureName, p);
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

	public static DataTable getAvgRateOrlastPurchaseRate(ReportsParameters obj)
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
				ParameterName = "@RateUomId",
				Value = obj.RateUOM
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
			if (obj.RefDocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocumentTypeId",
					Value = obj.RefDocumentTypeId
				});
			}
			if (obj.RefDocIdNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocIdNo",
					Value = obj.RefDocIdNo
				});
			}
			if (obj.RefDocSubId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RefDocSubIdNo",
					Value = obj.RefDocSubId
				});
			}
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("usp_getAvgRateOrlastPurchaseRate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DatabaseBackup(int OrganizationId, int CompanyId)
	{
		try
		{
			string Folderpath = string.Empty;
			Folderpath = CommonServies.GetConfigurationFromAllocation(OrganizationId, CompanyId, "Backup Folder Path");
			Cursor.Current = Cursors.Default;
			SqlConnection con = new SqlConnection(Services.GetConnectionString());
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(con.ConnectionString);
			sqlConnectionStringBuilder.InitialCatalog = "master";
			sqlConnectionStringBuilder.Password.ToString();
			SqlConnection con2 = new SqlConnection(sqlConnectionStringBuilder.ConnectionString.ToString());
			if (con2.State == ConnectionState.Closed)
			{
				con2.Open();
			}
			string dbbackup = con.Database.ToString();
			if (con.State.ToString() == "Open")
			{
				con.Close();
			}
			con.Open();
			string dbmappath = Folderpath + "\\" + dbbackup + "-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
			SqlCommand sqlCommand = new SqlCommand("BACKUP DATABASE [" + dbbackup + "] TO DISK='" + dbmappath + " .bak '", con2);
			sqlCommand.CommandTimeout = 600;
			sqlCommand.ExecuteNonQuery();
			con.Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static byte[] QrCodeGenerate(string QrCode)
	{
		try
		{
			byte[] byteImage = null;
			Bitmap bitMap = new QRCode(new QRCodeGenerator().CreateQrCode(QrCode, QRCodeGenerator.ECCLevel.Q)).GetGraphic(5);
			using (MemoryStream ms = new MemoryStream())
			{
				bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
				byteImage = ms.ToArray();
			}
			return byteImage;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByID(ReportsParameters obj)
	{
		try
		{
			new CommonServices().RecoredRemoveByOrgCompDocAndByID(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool GetERPFeatureById(int OrganizationId, int CompanyId, int FeatureId)
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

	public static DataTable GetAvgRateFromAvaiblableLoaderStock(ReportsParameters obj)
	{
		try
		{
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
				ParameterName = "@DocumentTypeId",
				Value = obj.RefDocumentTypeId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@RefDocIdNo",
				Value = obj.RefRefDocIdNo
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@RefDocSubIdNo",
				Value = obj.RefDocSubId
			});
			x.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAvgRateFromAvaiblableLoaderStock"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvSaleInvoice>("[Sp_InvSaleInvoice_GetAllMethod]", x);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getRatefromLastPurchase(ReportsParameters obj)
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
			if (obj.RecId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = obj.RecId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			return GenericProvider.GetDataTableProc<ConfigrationsAllocation>("pcc.usp_getRatefromLastPurchase", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
