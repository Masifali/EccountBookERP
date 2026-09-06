using System;
using System.IO;

namespace Architecture.WebHelper.Helper;

public static class LogHelper
{
	public static void Reportlogwriteondrive(string strMessage, bool IsClear = false)
	{
		try
		{
			if (IsClear)
			{
				File.WriteAllText("C:\\ReportLog.txt", string.Empty);
			}
			using StreamWriter sw = new StreamWriter("C:\\ReportLog.txt", append: true);
			sw.WriteLine(DateTime.Now.ToString() + "----" + strMessage);
		}
		catch
		{
		}
	}
}
