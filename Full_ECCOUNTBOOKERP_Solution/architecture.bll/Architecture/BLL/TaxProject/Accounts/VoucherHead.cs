using System;
using Architecture.DAL.TaxProject.Accounts;
using Architecture.Model.TaxProject.Accounts;

namespace Architecture.BLL.TaxProject.Accounts;

public class VoucherHead
{
	public static int Save(Architecture.Model.TaxProject.Accounts.VoucherHead obj)
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
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.TaxProject.Accounts.VoucherHead().SetData(obj, "Sp_VoucherHead_Insert");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.TaxProject.Accounts.VoucherHead().SetData(obj, "Sp_VoucherHead_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
