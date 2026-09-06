using System;
using System.Data;
using System.Windows.Forms;

namespace Architecture.WinApp.Helper;

public static class ComboBoxHelper
{
	public static DataTable GridComboBoxData(string RequestFor, DataGridViewComboBoxColumn cmb)
	{
		DataTable dt = null;
		try
		{
			if (RequestFor == "AccountCategory")
			{
				GetAccountCategory(cmb);
			}
		}
		catch (Exception)
		{
			throw;
		}
		return dt;
	}

	public static void GetAccountCategory(DataGridViewComboBoxColumn cmb)
	{
	}

	public static void AllMaterial(ComboBox cmb)
	{
	}

	public static void AllUnit(ComboBox cmb)
	{
	}

	public static void AllVoucherType(ComboBox cmb)
	{
	}
}
