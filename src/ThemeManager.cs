using System;
using System.Drawing;
using Architecture.WinApp.Properties;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace ThemeApp
{

public static class ThemeManager
{
	public static Color PanelColor { get; private set; }

	public static Color ButtonColor { get; private set; }

	public static event Action ThemeChanged;

	static ThemeManager()
	{
		LoadTheme();
	}

	public static void SetPanelColor(Color color)
	{
		PanelColor = color;
		ThemeManager.ThemeChanged?.Invoke();
	}

	public static void SetButtonColor(Color color)
	{
		ButtonColor = color;
		ThemeManager.ThemeChanged?.Invoke();
	}

	private static void SaveTheme(string key, Color color)
	{
		if (key == "PanelColor")
		{
			Settings.Default.PanelColor = ColorTranslator.ToHtml(color);
		}
		else if (key == "ButtonColor")
		{
			Settings.Default.ButtonColor = ColorTranslator.ToHtml(color);
		}
		Settings.Default.Save();
	}

	private static void LoadTheme()
	{
	}
}

}
