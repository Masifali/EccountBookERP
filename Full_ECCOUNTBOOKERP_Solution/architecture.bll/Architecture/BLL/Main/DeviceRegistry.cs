using System;
using Architecture.DAL.Main;
using Architecture.Model.Main;

namespace Architecture.BLL.Main;

public class DeviceRegistry
{
	public static int save(Architecture.Model.Main.DeviceRegistry cty)
	{
		try
		{
			return new Architecture.DAL.Main.DeviceRegistry().SetData(cty);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
