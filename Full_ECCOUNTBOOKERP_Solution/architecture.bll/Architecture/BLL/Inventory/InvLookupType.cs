using System;
using System.Collections.Generic;
using Architecture.DAL.Inventory;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Inventory;

public class InvLookupType
{
	public static int Save(Architecture.Model.Inventory.InvLookupType lookupType)
	{
		try
		{
			if (lookupType.Id == 0)
			{
				return new Architecture.DAL.Inventory.InvLookupType().Insert(lookupType);
			}
			return new Architecture.DAL.Inventory.InvLookupType().Update(lookupType);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Inventory.InvLookupType GetByID(int Id)
	{
		try
		{
			return new Architecture.DAL.Inventory.InvLookupType().ReadByID(Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Inventory.InvLookupType> GetAll(Architecture.Model.Inventory.InvLookupType lookupType)
	{
		try
		{
			return new Architecture.DAL.Inventory.InvLookupType().ReadAll(lookupType);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
