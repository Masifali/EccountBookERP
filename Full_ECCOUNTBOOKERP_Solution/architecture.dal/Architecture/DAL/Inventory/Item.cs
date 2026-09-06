using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.FeedMill.PricingSchedule;
using Architecture.Model.Inventory;
using Architecture.Model.POS;
using Architecture.Model.VPS;

namespace Architecture.DAL.Inventory;

public class Item : DALBase
{
	public int SetData(Architecture.Model.Inventory.Item obj, string ProcName, bool AutoCoaInsertIsOn)
	{
		List<string> copiedFiles = new List<string>();
		string AttachmentFolderPath = "";
		ServerCredentials Sc = new ServerCredentials();
		bool IsVpsAttachmentsServiceOn = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "IsVpsAttachmentsServiceOn"));
		bool AutoCoaDefineByItemNameOnInsert = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "AutoCoaDefineByItemNameOnInsert"));
		bool SameAccountForStockAndRevenue = Conversion.ToBool(CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "SameAccountForStockAndRevenue"));
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			if (CommonServices.GetParentCategoryIdByItemCategoryId(obj.OrganizationId, obj.CompanyId, obj.ItemCategoryId) != 9 && obj.Id == 0 && AutoCoaDefineByItemNameOnInsert && AutoCoaInsertIsOn)
			{
				int StockAccountId = 0;
				int RevenueAccountId = 0;
				int CategoryInventoryAccountId = 0;
				string CategoryInventoryAccountCode = "";
				int CategoryInventoryAccountClassId = 0;
				int CategoryInventoryAccountTypeId = 0;
				int CategoryInventoryBSNoteId = 0;
				int CategoryInventoryPLNoteId = 0;
				int CategoryRevenueAccountId = 0;
				string CategoryRevenueAccountCode = "";
				int CategoryRevenueAccountClassId = 0;
				int CategoryRevenueAccountTypeId = 0;
				int CategoryRevenueBSNoteId = 0;
				int CategoryRevenuePLNoteId = 0;
				if (SameAccountForStockAndRevenue)
				{
					List<SqlParameter> p3 = new List<SqlParameter>();
					p3.Add(new SqlParameter
					{
						ParameterName = "@ItemCategoryId",
						Value = obj.ItemCategoryId
					});
					p3.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "Stock"
					});
					DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("usp_getAccountsInfoByItemCategoryId", p3);
					if (dt != null && dt.Rows.Count > 0)
					{
						CategoryInventoryAccountId = Conversion.ToInt(dt.Rows[0]["ParentCodeId"]);
						CategoryInventoryAccountCode = Conversion.ToString(dt.Rows[0]["ParentAccountCode"]);
						CategoryInventoryAccountClassId = Conversion.ToInt(dt.Rows[0]["AccountClass"]);
						CategoryInventoryAccountTypeId = Conversion.ToInt(dt.Rows[0]["AccountTypeId"]);
						CategoryInventoryBSNoteId = Conversion.ToInt(dt.Rows[0]["BSNoteId"]);
						CategoryInventoryPLNoteId = Conversion.ToInt(dt.Rows[0]["PLNoteId"]);
						CategoryRevenueAccountId = Conversion.ToInt(dt.Rows[0]["ParentCodeId"]);
						CategoryRevenueAccountCode = Conversion.ToString(dt.Rows[0]["ParentAccountCode"]);
						CategoryRevenueAccountClassId = Conversion.ToInt(dt.Rows[0]["AccountClass"]);
						CategoryRevenueAccountTypeId = Conversion.ToInt(dt.Rows[0]["AccountTypeId"]);
						CategoryRevenueBSNoteId = Conversion.ToInt(dt.Rows[0]["BSNoteId"]);
						CategoryRevenuePLNoteId = Conversion.ToInt(dt.Rows[0]["PLNoteId"]);
					}
					RevenueAccountId = (StockAccountId = SaveCoa(CategoryInventoryAccountCode, CategoryInventoryAccountClassId, CategoryInventoryAccountId, CategoryInventoryAccountTypeId, CategoryInventoryPLNoteId, CategoryInventoryBSNoteId, obj, sqlTrn, "Stock A/c"));
				}
				else
				{
					List<SqlParameter> p4 = new List<SqlParameter>();
					p4.Add(new SqlParameter
					{
						ParameterName = "@ItemCategoryId",
						Value = obj.ItemCategoryId
					});
					p4.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "Stock"
					});
					DataTable dtStock = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("usp_getAccountsInfoByItemCategoryId", p4);
					if (dtStock != null && dtStock.Rows.Count > 0)
					{
						CategoryInventoryAccountId = Conversion.ToInt(dtStock.Rows[0]["ParentCodeId"]);
						CategoryInventoryAccountCode = Conversion.ToString(dtStock.Rows[0]["ParentAccountCode"]);
						CategoryInventoryAccountClassId = Conversion.ToInt(dtStock.Rows[0]["AccountClass"]);
						CategoryInventoryAccountTypeId = Conversion.ToInt(dtStock.Rows[0]["AccountTypeId"]);
						CategoryInventoryBSNoteId = Conversion.ToInt(dtStock.Rows[0]["BSNoteId"]);
						CategoryInventoryPLNoteId = Conversion.ToInt(dtStock.Rows[0]["PLNoteId"]);
					}
					p4[1].Value = "Sale";
					DataTable dtSale = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.Item>("usp_getAccountsInfoByItemCategoryId", p4);
					if (dtSale != null && dtSale.Rows.Count > 0)
					{
						CategoryRevenueAccountId = Conversion.ToInt(dtSale.Rows[0]["ParentCodeId"]);
						CategoryRevenueAccountCode = Conversion.ToString(dtSale.Rows[0]["ParentAccountCode"]);
						CategoryRevenueAccountClassId = Conversion.ToInt(dtSale.Rows[0]["AccountClass"]);
						CategoryRevenueAccountTypeId = Conversion.ToInt(dtSale.Rows[0]["AccountTypeId"]);
						CategoryRevenueBSNoteId = Conversion.ToInt(dtSale.Rows[0]["BSNoteId"]);
						CategoryRevenuePLNoteId = Conversion.ToInt(dtSale.Rows[0]["PLNoteId"]);
					}
					if (CategoryInventoryAccountId == CategoryRevenueAccountId)
					{
						StockAccountId = SaveCoa(CategoryInventoryAccountCode, CategoryInventoryAccountClassId, CategoryInventoryAccountId, CategoryInventoryAccountTypeId, CategoryInventoryPLNoteId, CategoryInventoryBSNoteId, obj, sqlTrn, "Stock A/c");
						RevenueAccountId = StockAccountId;
					}
					else
					{
						StockAccountId = SaveCoa(CategoryInventoryAccountCode, CategoryInventoryAccountClassId, CategoryInventoryAccountId, CategoryInventoryAccountTypeId, CategoryInventoryPLNoteId, CategoryInventoryBSNoteId, obj, sqlTrn, "Stock A/c");
						RevenueAccountId = SaveCoa(CategoryRevenueAccountCode, CategoryRevenueAccountClassId, CategoryRevenueAccountId, CategoryRevenueAccountTypeId, CategoryRevenuePLNoteId, CategoryRevenueBSNoteId, obj, sqlTrn, "Sale A/c");
					}
				}
				obj.PurchaseGLAC = StockAccountId;
				obj.SaleGLAC = RevenueAccountId;
				if (obj.PurchaseGLAC == 0)
				{
					throw new Exception("PurchaseGLAC  Account Not Found");
				}
				if (obj.SaleGLAC == 0)
				{
					throw new Exception("SaleGLAC Account Not Found");
				}
				if (SameAccountForStockAndRevenue && obj.PurchaseGLAC != obj.SaleGLAC)
				{
					throw new Exception("PurchaseGLAC And SaleGLAC Account should be same when Configuration 'SameAccountForStockAndRevenue' is on");
				}
			}
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				if (obj.ApplyGST && obj.TaxTypeId > 0)
				{
					Architecture.Model.Inventory.ItemTaxSchedule taxSchedule = new Architecture.Model.Inventory.ItemTaxSchedule();
					taxSchedule.ItemId = returnID;
					taxSchedule.IsActive = true;
					taxSchedule.EffectedDate = DateTime.Now;
					taxSchedule.EntryDate = DateTime.Now;
					taxSchedule.ModifyDate = DateTime.Now;
					taxSchedule.EntryUser = obj.EntryUser;
					taxSchedule.OrganizationId = obj.OrganizationId;
					taxSchedule.CompanyId = obj.CompanyId;
					taxSchedule.TaxTypeId = obj.TaxTypeId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, taxSchedule, "Sp_ItemTaxSchedule_Insert"));
				}
				if (obj.ItemGroupId > 0)
				{
					DataTable lst = null;
					List<SqlParameter> p2 = new List<SqlParameter>();
					p2.Add(new SqlParameter
					{
						ParameterName = "@ItemGroupId",
						Value = obj.ItemGroupId
					});
					p2.Add(new SqlParameter
					{
						ParameterName = "@Activity",
						Value = "ReadByItemGroupId"
					});
					lst = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.UOMSchedule>("Sp_UOMSchedule_GetAllMethod", p2);
					if (lst.Rows.Count > 0)
					{
						for (int i = 0; i < lst.Rows.Count; i++)
						{
							Architecture.Model.Inventory.UOMSchedule cat = new Architecture.Model.Inventory.UOMSchedule();
							cat.Equivalent = Conversion.ToDouble(lst.Rows[i]["Equivalent"]);
							cat.ScheduleUnitId = Conversion.ToInt(lst.Rows[i]["UOMId"]);
							cat.ScheduleUnitId = Conversion.ToInt(lst.Rows[i]["UOMId"]);
							cat.QtyEquivalent = Conversion.ToDouble(lst.Rows[i]["QtyEquivalent"]);
							cat.ItemId = Conversion.ToInt(returnID);
							cat.EntryDate = DateTime.Now;
							cat.ModifyDate = DateTime.Now;
							cat.EntryUser = obj.EntryUser;
							cat.OrganizationId = obj.OrganizationId;
							cat.CompanyId = obj.CompanyId;
							cat.Active = true;
							if (obj.ItemClassId == 8 && cat.Equivalent == 1000.0)
							{
								cat.BaseRateUom = true;
							}
							else if (cat.Equivalent == 40.0)
							{
								cat.BaseRateUom = true;
							}
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, cat, "Sp_UOMSchedule_Insert"));
						}
					}
				}
				else
				{
					Architecture.Model.Inventory.UOMSchedule cat2 = new Architecture.Model.Inventory.UOMSchedule();
					cat2.Equivalent = Conversion.ToDouble(obj.Equivalent);
					cat2.ScheduleUnitId = Conversion.ToInt(obj.BaseUnitId);
					cat2.ItemId = Conversion.ToInt(returnID);
					cat2.EntryDate = DateTime.Now;
					cat2.ModifyDate = DateTime.Now;
					cat2.EntryUser = obj.EntryUser;
					cat2.OrganizationId = obj.OrganizationId;
					cat2.CompanyId = obj.CompanyId;
					cat2.QtyEquivalent = 1.0;
					cat2.Active = true;
					if (obj.ItemClassId == 8 && cat2.Equivalent == 1000.0)
					{
						cat2.BaseRateUom = true;
					}
					else if (cat2.Equivalent == 40.0)
					{
						cat2.BaseRateUom = true;
					}
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, cat2, "Sp_UOMSchedule_Insert"));
				}
				if (obj.ItemAllocationlist == null || obj.ItemAllocationlist.Count <= 0)
				{
					throw new Exception("Item Allocation List Empty");
				}
				foreach (Architecture.Model.Inventory.ItemAllocation item4 in obj.ItemAllocationlist)
				{
					item4.ItemId = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_ItemAllocation_Insert"));
					if (obj.CostPrice > 0.0)
					{
						Architecture.Model.POS.ItemPricingSchedule itemPricing4 = new Architecture.Model.POS.ItemPricingSchedule();
						itemPricing4.ItemId = returnID;
						itemPricing4.EffectedDate = DateTime.Now;
						itemPricing4.EntryDate = DateTime.Now;
						itemPricing4.ModifyDate = DateTime.Now;
						itemPricing4.EntryUser = obj.EntryUser;
						itemPricing4.OrganizationId = item4.OrganizationId;
						itemPricing4.CompanyId = item4.CompanyId;
						itemPricing4.BranchesId = obj.BranchesId;
						itemPricing4.ItemPrice = obj.CostPrice;
						itemPricing4.PriceTypeId = 4;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing4, "Sp_ItemPricingSchedule_Insert"));
					}
					if (obj.PurchasePrice > 0.0)
					{
						Architecture.Model.POS.ItemPricingSchedule itemPricing3 = new Architecture.Model.POS.ItemPricingSchedule();
						itemPricing3.ItemId = returnID;
						itemPricing3.EffectedDate = DateTime.Now;
						itemPricing3.EntryDate = DateTime.Now;
						itemPricing3.ModifyDate = DateTime.Now;
						itemPricing3.EntryUser = obj.EntryUser;
						itemPricing3.OrganizationId = item4.OrganizationId;
						itemPricing3.CompanyId = item4.CompanyId;
						itemPricing3.BranchesId = obj.BranchesId;
						itemPricing3.ItemPrice = obj.PurchasePrice;
						itemPricing3.PriceTypeId = 5;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing3, "Sp_ItemPricingSchedule_Insert"));
					}
					if (obj.WholeSalePrice > 0.0)
					{
						Architecture.Model.POS.ItemPricingSchedule itemPricing2 = new Architecture.Model.POS.ItemPricingSchedule();
						itemPricing2.ItemId = returnID;
						itemPricing2.EffectedDate = DateTime.Now;
						itemPricing2.EntryDate = DateTime.Now;
						itemPricing2.ModifyDate = DateTime.Now;
						itemPricing2.EntryUser = obj.EntryUser;
						itemPricing2.OrganizationId = item4.OrganizationId;
						itemPricing2.CompanyId = item4.CompanyId;
						itemPricing2.BranchesId = obj.BranchesId;
						itemPricing2.ItemPrice = obj.WholeSalePrice;
						itemPricing2.PriceTypeId = 6;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing2, "Sp_ItemPricingSchedule_Insert"));
					}
					if (obj.RetailPrice > 0.0)
					{
						Architecture.Model.POS.ItemPricingSchedule itemPricing = new Architecture.Model.POS.ItemPricingSchedule();
						itemPricing.ItemId = returnID;
						itemPricing.EffectedDate = DateTime.Now;
						itemPricing.EntryDate = DateTime.Now;
						itemPricing.ModifyDate = DateTime.Now;
						itemPricing.EntryUser = obj.EntryUser;
						itemPricing.OrganizationId = item4.OrganizationId;
						itemPricing.CompanyId = item4.CompanyId;
						itemPricing.BranchesId = obj.BranchesId;
						itemPricing.ItemPrice = obj.RetailPrice;
						itemPricing.PriceTypeId = 7;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing, "Sp_ItemPricingSchedule_Insert"));
					}
					if (obj.ReorderLevel > 0.0)
					{
						Architecture.Model.Inventory.ItemsReorderSchedule itemsReorder = new Architecture.Model.Inventory.ItemsReorderSchedule();
						itemsReorder.ItemId = returnID;
						itemsReorder.EffectiveDate = DateTime.Now;
						itemsReorder.EntryDate = DateTime.Now;
						itemsReorder.ModifyDate = DateTime.Now;
						itemsReorder.EntryUser = obj.EntryUser;
						itemsReorder.OrganizationId = item4.OrganizationId;
						itemsReorder.CompanyId = item4.CompanyId;
						itemsReorder.BranchesId = obj.BranchesId;
						itemsReorder.MinQty = obj.MinStockLevel;
						itemsReorder.MaxQty = obj.MaxStockLevel;
						itemsReorder.OptimalQty = obj.OptimalStockLevel;
						itemsReorder.ReOrderPoint = obj.ReorderLevel;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemsReorder, "Sp_ItemsReorderSchedule_Insert"));
					}
					if (obj.CommOnSale || obj.CommOnPurchase)
					{
						Architecture.Model.Inventory.ItemWiseCommissionSchedule itemComm = new Architecture.Model.Inventory.ItemWiseCommissionSchedule();
						itemComm.CompanyId = item4.CompanyId;
						itemComm.OrgnizationId = item4.OrganizationId;
						itemComm.BranchesId = item4.BranchId;
						itemComm.EntryUserId = obj.EntryUser;
						itemComm.ModifyUserId = obj.ModifyUser;
						itemComm.ModifyDate = DateTime.Now;
						itemComm.EntryDate = DateTime.Now;
						itemComm.Id = Conversion.ToInt(0);
						itemComm.ItemId = returnID;
						itemComm.OnSale = obj.CommOnSale;
						itemComm.OnPurchase = obj.CommOnPurchase;
						itemComm.EffectedFromDate = clsGlobalVariables.ActiveYr.Start_Period;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemComm, "[dbo].[USP_ItemWiseCommission_InsertOrUpdate]"));
					}
					Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule fedpricing = new Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule();
					fedpricing.ItemId = returnID;
					fedpricing.EffectiveDate = DateTime.Now.AddDays(-7.0);
					fedpricing.ItemRate = 1.0;
					fedpricing.PricingCustomGroupId = 1;
					fedpricing.IsActive = true;
					fedpricing.DocumentTypeId = 1;
					fedpricing.EntryDate = DateTime.Now;
					fedpricing.ModifyDate = DateTime.Now;
					fedpricing.EntryUserId = obj.EntryUser;
					fedpricing.ModifyUserId = obj.ModifyUser;
					fedpricing.IsApproved = false;
					fedpricing.ApprovedUserId = 0;
					fedpricing.ApprovedDate = DateTime.Now;
					fedpricing.FinancialYearId = 0;
					fedpricing.OrganizationId = item4.OrganizationId;
					fedpricing.CompanyId = item4.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, fedpricing, "[fed].[usp_ItemPricingSchedule_Insert]"));
				}
			}
			else
			{
				returnID = obj.Id;
			}
			if (IsVpsAttachmentsServiceOn)
			{
				Sc = CommonServices.GetServerCredentials();
			}
			else
			{
				AttachmentFolderPath = CommonServices.GetConfigurationFromAllocation(obj.OrganizationId, obj.CompanyId, "Attachment Folder Path");
				if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0 && !Directory.Exists(AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				obj.AttachmentsList = CommonServices.CommonMethodForAttachmentsToPhysicallySave(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.AttachmentsList, copiedFiles);
			}
			if (obj.ItemImageslist != null && obj.ItemImageslist.Count > 0)
			{
				double FileSize = 0.0;
				string FileFullPath = "";
				foreach (ItemImage image in obj.ItemImageslist)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						if (!Directory.Exists(AttachmentFolderPath))
						{
							throw new Exception("Attachment Folder Path Not Configure Properly");
						}
						if (image.ImagePath != null && image.ImagePath != string.Empty)
						{
							image.FileName = image.FileName;
							image.ImagePath = image.ImagePath;
							continue;
						}
						FileFullPath = image.FileName;
						if (!File.Exists(System.IO.Path.Combine(AttachmentFolderPath, System.IO.Path.GetFileName(image.ImagePath))))
						{
							image.FileName = System.IO.Path.GetFileName(FileFullPath);
							string UniquePath = $"{Guid.NewGuid()}.Jpg";
							image.ImagePath = UniquePath;
							FileInfo info = new FileInfo(FileFullPath);
							if (info.Exists)
							{
								FileSize = (double)info.Length / 1048576.0;
							}
							if (FileSize > 5.0)
							{
								throw new Exception("File Size Exceeds 5MB Of File: " + image.FileName);
							}
							string SavePath = System.IO.Path.Combine(AttachmentFolderPath, image.ImagePath);
							if (!File.Exists(SavePath))
							{
								File.Copy(FileFullPath, SavePath, overwrite: true);
								copiedFiles.Add(SavePath);
							}
						}
						else
						{
							image.FileName = System.IO.Path.GetFileName(image.FileName);
							image.ImagePath = image.ImagePath;
						}
						continue;
					}
					if (image.ImagePath != null && image.ImagePath != string.Empty)
					{
						image.FileName = image.FileName;
						image.ImagePath = image.ImagePath;
						continue;
					}
					bool FileExist = false;
					string uri2 = "ftp://" + Sc.ServerIp + "/" + System.IO.Path.GetFileName(image.FileName);
					try
					{
						FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri2);
						obj2.Method = "MDTM";
						obj2.UsePassive = true;
						obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using ((FtpWebResponse)obj2.GetResponse())
						{
							FileExist = true;
						}
					}
					catch (WebException ex3)
					{
						if (ex3.Response is FtpWebResponse { StatusCode: not FtpStatusCode.ActionNotTakenFileUnavailable })
						{
							throw new Exception(ex3.Message);
						}
					}
					if (!FileExist)
					{
						FileFullPath = image.FileName;
						image.FileName = System.IO.Path.GetFileName(FileFullPath);
						string UniquePath2 = $"{Guid.NewGuid()}.Jpg";
						image.ImagePath = UniquePath2;
						FileInfo info2 = new FileInfo(FileFullPath);
						if (info2.Exists)
						{
							FileSize = (double)info2.Length / 1048576.0;
						}
						if (FileSize > 5.0)
						{
							throw new Exception("File Size Exceeds 5MB Of File: " + image.FileName);
						}
						string url = "ftp://" + Sc.ServerIp + "/" + image.ImagePath;
						try
						{
							FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
							request.Method = "STOR";
							request.UsePassive = true;
							request.UseBinary = true;
							request.KeepAlive = false;
							request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							byte[] fileContents;
							using (FileStream fileStream = info2.OpenRead())
							{
								fileContents = new byte[info2.Length];
								fileStream.Read(fileContents, 0, fileContents.Length);
							}
							request.ContentLength = fileContents.Length;
							using (Stream requestStream = request.GetRequestStream())
							{
								requestStream.Write(fileContents, 0, fileContents.Length);
							}
							using ((FtpWebResponse)request.GetResponse())
							{
								copiedFiles.Add(image.ImagePath);
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
						catch (Exception ex2)
						{
							throw new Exception("Error: " + ex2.Message);
						}
					}
					else
					{
						image.FileName = System.IO.Path.GetFileName(image.FileName);
						image.ImagePath = image.ImagePath;
					}
				}
			}
			if (obj.ItemImageslist != null && obj.ItemImageslist.Count > 0)
			{
				foreach (ItemImage item3 in obj.ItemImageslist)
				{
					item3.ItemId = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "USP_ItemImage_Insert"));
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
				foreach (DMSAttachments item2 in obj.AttachmentsList.ToList())
				{
					item2.RefAccountId = obj.ItemTypeId;
					item2.RefDocumentTypeId = 0;
					item2.RefDocumentNo = returnID;
					item2.EntryDate = DateTime.Now;
					item2.ModifyDate = DateTime.Now;
					item2.EntryUser = obj.EntryUser;
					item2.ModifyUser = obj.ModifyUser;
					item2.OrganizationId = obj.OrganizationId;
					item2.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Proc_DMSAttachments_Insert"));
				}
			}
			if (obj.DeleteAttachmentsList != null && obj.DeleteAttachmentsList.Count > 0)
			{
				CommonServices.DeleteAttachmentsPhysically(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, obj.DeleteAttachmentsList);
			}
			if (obj.ItemImagesDeletelist != null && obj.ItemImagesDeletelist.Count > 0)
			{
				foreach (ItemImage item in obj.ItemImagesDeletelist)
				{
					if (!IsVpsAttachmentsServiceOn)
					{
						string Path = AttachmentFolderPath + "\\" + item.ImagePath;
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
					string uri = "ftp://" + Sc.ServerIp + "/" + item.ImagePath;
					try
					{
						FtpWebRequest obj3 = (FtpWebRequest)WebRequest.Create(uri);
						obj3.Method = "DELE";
						obj3.UsePassive = true;
						obj3.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse deleteResponse = (FtpWebResponse)obj3.GetResponse();
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
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex5)
		{
			sqlTrn.Rollback();
			CommonServices.DeleteAttachmentsPhysicallyInException(Sc, IsVpsAttachmentsServiceOn, AttachmentFolderPath, copiedFiles);
			throw ex5;
		}
	}

	private int SaveCoa(string CategoryInventoryAccountCode, int CategoryInventoryAccountClassId, int CategoryInventoryAccountId, int CategoryInventoryAccountTypeId, int CategoryInventoryPLNoteId, int CategoryInventoryBSNoteId, Architecture.Model.Inventory.Item objitem, SqlTransaction sqlTrn, string Description)
	{
		int returnID = 0;
		try
		{
			bool BranchFeatureFinancial = CommonServices.GetERPFeaturesByCompanyId(objitem.OrganizationId, objitem.CompanyId, 17);
			ChartofAccount obj = new ChartofAccount();
			obj.ParentAccountCode = CategoryInventoryAccountCode;
			obj.AccountClass = CategoryInventoryAccountClassId;
			obj.ParentCodeId = CategoryInventoryAccountId;
			obj.Account_Level = 4;
			obj.AccountTitle = Conversion.ToString(objitem.ItemName) + " " + Description;
			obj.AccountGroup = Conversion.ToString("Detail");
			obj.AccountTypeId = CategoryInventoryAccountTypeId;
			obj.PLNoteId = CategoryInventoryPLNoteId;
			obj.BSNoteId = CategoryInventoryBSNoteId;
			obj.OtherErpCode = Conversion.ToString(objitem.ItemCode);
			obj.OrganizationId = objitem.OrganizationId;
			obj.CompanyId = objitem.CompanyId;
			obj.BranchId = objitem.BranchesId;
			obj.EntryUser = objitem.EntryUser;
			obj.IsActive = true;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (obj.AccountGroup == "Detail")
			{
				obj.COAAllocationList = new List<COAAllocation>();
				int countfalsevalue = 0;
				int count = objitem.ItemAllocationlist.Count;
				if (count == 0)
				{
					throw new Exception("Item Allocation Grid Record Not Found");
				}
				for (int i = 0; i < objitem.ItemAllocationlist.Count; i++)
				{
					if (count == 1)
					{
						COAAllocation allocation = new COAAllocation();
						allocation.CompanyId = Conversion.ToInt(objitem.ItemAllocationlist[i].CompanyId);
						allocation.IsActive = true;
						allocation.BranchId = objitem.BranchesId;
						obj.COAAllocationList.Add(allocation);
					}
					else if (objitem.ItemAllocationlist[i].IsActive)
					{
						COAAllocation allocation2 = new COAAllocation();
						allocation2.CompanyId = Conversion.ToInt(objitem.ItemAllocationlist[i].CompanyId);
						allocation2.IsActive = true;
						allocation2.BranchId = objitem.BranchesId;
						obj.COAAllocationList.Add(allocation2);
					}
					else
					{
						countfalsevalue++;
					}
				}
				if (count > 1 && count == countfalsevalue)
				{
					throw new Exception("Please select Branch first");
				}
			}
			if (!string.IsNullOrEmpty(obj.ParentAccountCode))
			{
				obj.AccountCode = obj.ParentAccountCode;
			}
			if (obj.ParentAccountCode == "" || obj.ParentAccountCode == null)
			{
				obj.ParentAccountCode = "0";
			}
			obj.ActionId = 1;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, "Proc_ChartofAccount_Insert"));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
				if (obj.COAAllocationList != null && obj.COAAllocationList.Count > 0)
				{
					foreach (COAAllocation item in obj.COAAllocationList)
					{
						item.ChartofAccountId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_COAAllocation_Insert"));
						if (obj.AccountGroup == "Detail")
						{
							AccountsOpeningBalances balances = new AccountsOpeningBalances();
							balances.OrganizationId = obj.OrganizationId;
							balances.CompanyId = item.CompanyId;
							balances.EntryUser = obj.EntryUser;
							balances.ModifyUser = obj.ModifyUser;
							balances.EntryDate = DateTime.Now;
							balances.ModifyDate = DateTime.Now;
							balances.PostDate = DateTime.Now;
							balances.PostState = false;
							balances.ChartOfAccountId = returnID;
							balances.ChartOfAccountTitle = obj.AccountTitle;
							balances.FinancialYearId = obj.FinancialYearId;
							balances.YearObDebit = (BranchFeatureFinancial ? 0.0 : obj.YearObDebit);
							balances.YearObCredit = (BranchFeatureFinancial ? 0.0 : obj.YearObCredit);
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, balances, "Sp_AccountsOpeningBalances_Insert"));
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
		return returnID;
	}

	public int SaveList(ItemList ItemsList, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (Architecture.Model.Inventory.Item obj in ItemsList.ItemsList)
			{
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
				if (returnID > 0)
				{
					if (obj.ApplyGST && obj.TaxTypeId > 0)
					{
						Architecture.Model.Inventory.ItemTaxSchedule taxSchedule = new Architecture.Model.Inventory.ItemTaxSchedule();
						taxSchedule.ItemId = returnID;
						taxSchedule.IsActive = true;
						taxSchedule.EffectedDate = DateTime.Now;
						taxSchedule.EntryDate = DateTime.Now;
						taxSchedule.ModifyDate = DateTime.Now;
						taxSchedule.EntryUser = obj.EntryUser;
						taxSchedule.OrganizationId = obj.OrganizationId;
						taxSchedule.CompanyId = obj.CompanyId;
						taxSchedule.TaxTypeId = obj.TaxTypeId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, taxSchedule, "Sp_ItemTaxSchedule_Insert"));
					}
					if (obj.ItemAllocationlist == null || obj.ItemAllocationlist.Count <= 0)
					{
						throw new Exception("Item Allocation List Empty");
					}
					foreach (Architecture.Model.Inventory.ItemAllocation item in obj.ItemAllocationlist)
					{
						item.ItemId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ItemAllocation_Insert"));
						if (obj.ItemGroupId > 0)
						{
							DataTable lst = null;
							List<SqlParameter> p = new List<SqlParameter>();
							p.Add(new SqlParameter
							{
								ParameterName = "@ItemGroupId",
								Value = obj.ItemGroupId
							});
							p.Add(new SqlParameter
							{
								ParameterName = "@Activity",
								Value = "ReadByItemGroupId"
							});
							lst = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.UOMSchedule>("Sp_UOMSchedule_GetAllMethod", p);
							if (lst.Rows.Count > 0)
							{
								for (int i = 0; i < lst.Rows.Count; i++)
								{
									Architecture.Model.Inventory.UOMSchedule cat = new Architecture.Model.Inventory.UOMSchedule();
									cat.Equivalent = Conversion.ToSingle(lst.Rows[i]["Equivalent"]);
									cat.ScheduleUnitId = Conversion.ToInt(lst.Rows[i]["UOMId"]);
									cat.ItemId = Conversion.ToInt(returnID);
									cat.EntryDate = DateTime.Now;
									cat.ModifyDate = DateTime.Now;
									cat.EntryUser = obj.EntryUser;
									cat.OrganizationId = item.OrganizationId;
									cat.CompanyId = item.CompanyId;
									cat.Active = true;
									Convert.ToInt32(GenericProvider.SetProc(sqlTrn, cat, "Sp_UOMSchedule_Insert"));
								}
							}
						}
						else
						{
							Architecture.Model.Inventory.UOMSchedule cat2 = new Architecture.Model.Inventory.UOMSchedule();
							cat2.Equivalent = Conversion.ToDouble(obj.Equivalent);
							cat2.ScheduleUnitId = Conversion.ToInt(obj.BaseUnitId);
							cat2.ItemId = Conversion.ToInt(returnID);
							cat2.EntryDate = DateTime.Now;
							cat2.ModifyDate = DateTime.Now;
							cat2.EntryUser = obj.EntryUser;
							cat2.OrganizationId = item.OrganizationId;
							cat2.CompanyId = item.CompanyId;
							cat2.Active = true;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, cat2, "Sp_UOMSchedule_Insert"));
						}
						if (obj.CostPrice > 0.0)
						{
							Architecture.Model.POS.ItemPricingSchedule itemPricing4 = new Architecture.Model.POS.ItemPricingSchedule();
							itemPricing4.ItemId = returnID;
							itemPricing4.EffectedDate = DateTime.Now;
							itemPricing4.EntryDate = DateTime.Now;
							itemPricing4.ModifyDate = DateTime.Now;
							itemPricing4.EntryUser = obj.EntryUser;
							itemPricing4.OrganizationId = item.OrganizationId;
							itemPricing4.CompanyId = item.CompanyId;
							itemPricing4.BranchesId = obj.BranchesId;
							itemPricing4.ItemPrice = obj.CostPrice;
							itemPricing4.PriceTypeId = 4;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing4, "Sp_ItemPricingSchedule_Insert"));
						}
						if (obj.PurchasePrice > 0.0)
						{
							Architecture.Model.POS.ItemPricingSchedule itemPricing3 = new Architecture.Model.POS.ItemPricingSchedule();
							itemPricing3.ItemId = returnID;
							itemPricing3.EffectedDate = DateTime.Now;
							itemPricing3.EntryDate = DateTime.Now;
							itemPricing3.ModifyDate = DateTime.Now;
							itemPricing3.EntryUser = obj.EntryUser;
							itemPricing3.OrganizationId = item.OrganizationId;
							itemPricing3.CompanyId = item.CompanyId;
							itemPricing3.BranchesId = obj.BranchesId;
							itemPricing3.ItemPrice = obj.PurchasePrice;
							itemPricing3.PriceTypeId = 5;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing3, "Sp_ItemPricingSchedule_Insert"));
						}
						if (obj.WholeSalePrice > 0.0)
						{
							Architecture.Model.POS.ItemPricingSchedule itemPricing2 = new Architecture.Model.POS.ItemPricingSchedule();
							itemPricing2.ItemId = returnID;
							itemPricing2.EffectedDate = DateTime.Now;
							itemPricing2.EntryDate = DateTime.Now;
							itemPricing2.ModifyDate = DateTime.Now;
							itemPricing2.EntryUser = obj.EntryUser;
							itemPricing2.OrganizationId = item.OrganizationId;
							itemPricing2.CompanyId = item.CompanyId;
							itemPricing2.BranchesId = obj.BranchesId;
							itemPricing2.ItemPrice = obj.WholeSalePrice;
							itemPricing2.PriceTypeId = 6;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing2, "Sp_ItemPricingSchedule_Insert"));
						}
						if (obj.RetailPrice > 0.0)
						{
							Architecture.Model.POS.ItemPricingSchedule itemPricing = new Architecture.Model.POS.ItemPricingSchedule();
							itemPricing.ItemId = returnID;
							itemPricing.EffectedDate = DateTime.Now;
							itemPricing.EntryDate = DateTime.Now;
							itemPricing.ModifyDate = DateTime.Now;
							itemPricing.EntryUser = obj.EntryUser;
							itemPricing.OrganizationId = item.OrganizationId;
							itemPricing.CompanyId = item.CompanyId;
							itemPricing.BranchesId = obj.BranchesId;
							itemPricing.ItemPrice = obj.RetailPrice;
							itemPricing.PriceTypeId = 7;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemPricing, "Sp_ItemPricingSchedule_Insert"));
						}
						if (obj.ReorderLevel > 0.0)
						{
							Architecture.Model.Inventory.ItemsReorderSchedule itemsReorder = new Architecture.Model.Inventory.ItemsReorderSchedule();
							itemsReorder.ItemId = returnID;
							itemsReorder.EffectiveDate = DateTime.Now;
							itemsReorder.EntryDate = DateTime.Now;
							itemsReorder.ModifyDate = DateTime.Now;
							itemsReorder.EntryUser = obj.EntryUser;
							itemsReorder.OrganizationId = item.OrganizationId;
							itemsReorder.CompanyId = item.CompanyId;
							itemsReorder.BranchesId = obj.BranchesId;
							itemsReorder.MinQty = obj.MinStockLevel;
							itemsReorder.MaxQty = obj.MaxStockLevel;
							itemsReorder.OptimalQty = obj.OptimalStockLevel;
							itemsReorder.ReOrderPoint = obj.ReorderLevel;
							Convert.ToInt32(GenericProvider.SetProc(sqlTrn, itemsReorder, "Sp_ItemsReorderSchedule_Insert"));
						}
					}
				}
				else
				{
					returnID = obj.Id;
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

	public List<Architecture.Model.Inventory.Item> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Inventory.Item> lst = new List<Architecture.Model.Inventory.Item>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Inventory.Item>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.Inventory.Item item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ItemImagesByItemId"));
					item.ItemImageslist = GenericProvider.GetAllDetail<ItemImage>(sqlcon, "Sp_Item_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public int UpdateItemName(ReportsParameters obj)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("Sp_Item_GetAllMethod");
			sqlDataBase.AddInParameter(cmd, "Id", SqlDbType.Int, obj.Id);
			sqlDataBase.AddInParameter(cmd, "Activity", SqlDbType.NVarChar, "Update");
			sqlDataBase.AddInParameter(cmd, "ItemName", SqlDbType.NVarChar, obj.AccountTitle);
			sqlDataBase.AddInParameter(cmd, "ItemNameOtherLingo", SqlDbType.NVarChar, obj.OtherCode);
			sqlDataBase.AddInParameter(cmd, "Code", SqlDbType.NVarChar, obj.OrderNo);
			return sqlDataBase.ExecuteNonQuery(cmd);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int ContractorWagesItemSetData(Architecture.Model.Inventory.Item obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				foreach (Architecture.Model.Inventory.ItemAllocation item in obj.ItemAllocationlist)
				{
					item.ItemId = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_ItemAllocation_Insert"));
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

	public int SetData(MasterItems obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public int SetData(MasterItemAllocations obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (MasterItemAllocations item in obj.MasterItemAllocationslist)
			{
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, ProcName));
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
}
