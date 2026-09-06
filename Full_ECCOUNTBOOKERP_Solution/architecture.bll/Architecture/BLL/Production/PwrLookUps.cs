using System;
using Architecture.DAL.Production;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class PwrLookUps
{
	public static int Save(Architecture.Model.Production.PwrLookUps obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Production.PwrLookUps().SetDate(obj, "Sp_PwrEngyLookUps_Insert");
			}
			return new Architecture.DAL.Production.PwrLookUps().SetDate(obj, "Sp_PwrEngyLookUps_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
