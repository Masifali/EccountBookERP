// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Architecture.BLL;
using Architecture.BLL.Authentication;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{
using ActivityLog = Architecture.Model.Authentication.ActivityLog;
using CLV = Architecture.Model.CLV;

public class License
{
	public string ReturnKey(string Key)
	{
		try
		{
			string EncryptionKey = "AMIR2SPSCS44602";
			byte[] cipherBytes = Convert.FromBase64String(Key);
			using (Aes encryptor = Aes.Create()) {
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[13]
			{
				73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
				100, 101, 118
			});
			encryptor.Key = pdb.GetBytes(32);
			encryptor.IV = pdb.GetBytes(16);
			using (MemoryStream ms = new MemoryStream()) {
			using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cs.Write(cipherBytes, 0, cipherBytes.Length);
				cs.Close();
			}
			return Encoding.Unicode.GetString(ms.ToArray());
			}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public bool ChkLicense(global::Architecture.Model.Authentication.ActivityLog ActivityLog, List<global::Architecture.Model.CLV> CLV)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			bool Validate = false;
			List<ActivityLog> lst = global::Architecture.BLL.Authentication.ActivityLog.GetDate(ActivityLog);
			if (lst.Count == 0)
			{
				global::Architecture.BLL.Authentication.ActivityLog.Save(new ActivityLog
				{
					CompanyId = ActivityLog.CompanyId,
					OrganizationId = ActivityLog.OrganizationId,
					LoginDate = DateTime.Today,
					Activity = "Login"
				});
			}
			List<ActivityLog> DateList = lst;
			DateTime GetDate = ((!Conversion.CheckDateTimeNull(DateList[0].LoginDate)) ? Conversion.ToDateTime((object)DateList[0].LoginDate) : DateTime.Today);
			CLV = global::Architecture.BLL.CLV.GetLicense(lst[0].OrganizationId, lst[0].CompanyId).ToList();
			if (CLV.Count == 0)
			{
				return Validate;
			}
			string Company = CLV[0].CompanyName.ToString();
			string Key = CLV[0].LicenseKey.ToString();
			Key = ReturnKey(Key);
			string[] multiArray = Key.Split('(');
			if (multiArray.Length != 0)
			{
				if (multiArray[0].ToString() == Company)
				{
					DateTime LicenseDate = DateTime.Parse(multiArray[1].ToString());
					if (DateTime.Today < GetDate)
					{
						return Validate;
					}
					if (LicenseDate <= DateTime.Today)
					{
						return Validate;
					}
					return Validate = true;
				}
				return Validate;
			}
			return Validate;
		}
		catch (Exception)
		{
			return false;
		}
	}
}

}
