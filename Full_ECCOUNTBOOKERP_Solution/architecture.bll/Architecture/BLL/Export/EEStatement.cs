using System;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class EEStatement
{
	public static int Save(Architecture.Model.Export.EEStatement obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			_ = obj.Id;
			return new Architecture.DAL.Export.EEStatement().SetData(obj, "USP_EEStatement_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
