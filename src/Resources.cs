using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Properties
{

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Architecture.WinApp.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap _123 => (Bitmap)ResourceManager.GetObject("123", resourceCulture);

	internal static Bitmap _1231 => (Bitmap)ResourceManager.GetObject("1231", resourceCulture);

	internal static Bitmap _55download => (Bitmap)ResourceManager.GetObject("55download", resourceCulture);

	internal static Bitmap Account_Defination => (Bitmap)ResourceManager.GetObject("Account Defination", resourceCulture);

	internal static Bitmap Account_Reports => (Bitmap)ResourceManager.GetObject("Account Reports", resourceCulture);

	internal static Bitmap Account_Transaction => (Bitmap)ResourceManager.GetObject("Account Transaction", resourceCulture);

	internal static Bitmap AccountsCardIcon => (Bitmap)ResourceManager.GetObject("AccountsCardIcon", resourceCulture);

	internal static Bitmap Admin_Panel => (Bitmap)ResourceManager.GetObject("Admin Panel", resourceCulture);

	internal static Bitmap AlliedBank_PK => (Bitmap)ResourceManager.GetObject("AlliedBank_PK", resourceCulture);

	internal static Bitmap Analytic_Dashboard => (Bitmap)ResourceManager.GetObject("Analytic Dashboard", resourceCulture);

	internal static Bitmap arrow_down_s_line => (Bitmap)ResourceManager.GetObject("arrow-down-s-line", resourceCulture);

	internal static Bitmap arrow_left_fill => (Bitmap)ResourceManager.GetObject("arrow-left-fill", resourceCulture);

	internal static Bitmap arrow_left_line => (Bitmap)ResourceManager.GetObject("arrow-left-line", resourceCulture);

	internal static Bitmap arrow_right_circle_fill => (Bitmap)ResourceManager.GetObject("arrow-right-circle-fill", resourceCulture);

	internal static Bitmap arrow_right_fill => (Bitmap)ResourceManager.GetObject("arrow-right-fill", resourceCulture);

	internal static Bitmap arrow_right_line => (Bitmap)ResourceManager.GetObject("arrow-right-line", resourceCulture);

	internal static Bitmap arrow_right_line1 => (Bitmap)ResourceManager.GetObject("arrow-right-line1", resourceCulture);

	internal static Bitmap arrow_up_s_line => (Bitmap)ResourceManager.GetObject("arrow-up-s-line", resourceCulture);

	internal static Bitmap AskariBank_PK => (Bitmap)ResourceManager.GetObject("AskariBank_PK", resourceCulture);

	internal static Bitmap BackWord => (Bitmap)ResourceManager.GetObject("BackWord", resourceCulture);

	internal static Bitmap BankAlfalah_PK => (Bitmap)ResourceManager.GetObject("BankAlfalah_PK", resourceCulture);

	internal static Bitmap BankAlHabibLimited_PK => (Bitmap)ResourceManager.GetObject("BankAlHabibLimited_PK", resourceCulture);

	internal static Bitmap BOP => (Bitmap)ResourceManager.GetObject("BOP", resourceCulture);

	internal static Bitmap chat_history_fill => (Bitmap)ResourceManager.GetObject("chat-history-fill", resourceCulture);

	internal static Bitmap check_line => (Bitmap)ResourceManager.GetObject("check-line", resourceCulture);

	internal static Bitmap cheque_preview_background => (Bitmap)ResourceManager.GetObject("cheque_preview_background", resourceCulture);

	internal static Bitmap chequeplaceholder => (Bitmap)ResourceManager.GetObject("chequeplaceholder", resourceCulture);

	internal static Bitmap close_line => (Bitmap)ResourceManager.GetObject("close-line", resourceCulture);

	internal static Bitmap ColorPicker => (Bitmap)ResourceManager.GetObject("ColorPicker", resourceCulture);

	internal static Bitmap Contractor_Wages => (Bitmap)ResourceManager.GetObject("Contractor Wages", resourceCulture);

	internal static Bitmap CRM => (Bitmap)ResourceManager.GetObject("CRM", resourceCulture);

	internal static Bitmap Customer_Sales => (Bitmap)ResourceManager.GetObject("Customer Sales", resourceCulture);

	internal static Bitmap Dashboards => (Bitmap)ResourceManager.GetObject("Dashboards", resourceCulture);

	internal static Bitmap delete_bin_2_line => (Bitmap)ResourceManager.GetObject("delete-bin-2-line", resourceCulture);

	internal static Bitmap DeliveryCardIcon => (Bitmap)ResourceManager.GetObject("DeliveryCardIcon", resourceCulture);

	internal static Bitmap download => (Bitmap)ResourceManager.GetObject("download", resourceCulture);

	internal static Bitmap download_line => (Bitmap)ResourceManager.GetObject("download-line", resourceCulture);

	internal static Bitmap download_removebg_previewd => (Bitmap)ResourceManager.GetObject("download-removebg-previewd", resourceCulture);

	internal static Bitmap download1 => (Bitmap)ResourceManager.GetObject("download1", resourceCulture);

	internal static Bitmap DummyBank => (Bitmap)ResourceManager.GetObject("DummyBank", resourceCulture);

	internal static Bitmap DummyUser => (Bitmap)ResourceManager.GetObject("DummyUser", resourceCulture);

	internal static Bitmap execution => (Bitmap)ResourceManager.GetObject("execution", resourceCulture);

	internal static Bitmap Export => (Bitmap)ResourceManager.GetObject("Export", resourceCulture);

	internal static Bitmap Export_Reports => (Bitmap)ResourceManager.GetObject("Export Reports", resourceCulture);

	internal static Bitmap eye_fill => (Bitmap)ResourceManager.GetObject("eye-fill", resourceCulture);

	internal static Bitmap Fav_Star_Empty => (Bitmap)ResourceManager.GetObject("Fav_Star_Empty", resourceCulture);

	internal static Bitmap Fav_Star_Fill => (Bitmap)ResourceManager.GetObject("Fav_Star_Fill", resourceCulture);

	internal static Bitmap FaysalBankIslamic_PK => (Bitmap)ResourceManager.GetObject("FaysalBankIslamic_PK", resourceCulture);

	internal static Bitmap filter_line => (Bitmap)ResourceManager.GetObject("filter-line", resourceCulture);

	internal static Bitmap Finance_Management => (Bitmap)ResourceManager.GetObject("Finance Management", resourceCulture);

	internal static Bitmap Fixed_Assets => (Bitmap)ResourceManager.GetObject("Fixed Assets", resourceCulture);

	internal static Bitmap Forward => (Bitmap)ResourceManager.GetObject("Forward", resourceCulture);

	internal static Bitmap Forward2 => (Bitmap)ResourceManager.GetObject("Forward2", resourceCulture);

	internal static Bitmap Forward2_removebg_preview => (Bitmap)ResourceManager.GetObject("Forward2-removebg-preview", resourceCulture);

	internal static Bitmap google_forms => (Bitmap)ResourceManager.GetObject("google-forms", resourceCulture);

	internal static Bitmap GoToFirst => (Bitmap)ResourceManager.GetObject("GoToFirst", resourceCulture);

	internal static Bitmap HabibBank_HBL_PK => (Bitmap)ResourceManager.GetObject("HabibBank_HBL_PK", resourceCulture);

	internal static Bitmap HABIBMETRO_PK => (Bitmap)ResourceManager.GetObject("HABIBMETRO_PK", resourceCulture);

	internal static Bitmap heart_3_fill => (Bitmap)ResourceManager.GetObject("heart-3-fill", resourceCulture);

	internal static Bitmap heart_3_line => (Bitmap)ResourceManager.GetObject("heart-3-line", resourceCulture);

	internal static Bitmap heart_fill => (Bitmap)ResourceManager.GetObject("heart-fill", resourceCulture);

	internal static Bitmap Hi_Hand => (Bitmap)ResourceManager.GetObject("Hi_Hand", resourceCulture);

	internal static Bitmap hide => (Bitmap)ResourceManager.GetObject("hide", resourceCulture);

	internal static Bitmap home_4_fill => (Bitmap)ResourceManager.GetObject("home-4-fill", resourceCulture);

	internal static Bitmap HRM => (Bitmap)ResourceManager.GetObject("HRM", resourceCulture);

	internal static Bitmap icons8_add_17 => (Bitmap)ResourceManager.GetObject("icons8_add_17", resourceCulture);

	internal static Bitmap icons8_add_20 => (Bitmap)ResourceManager.GetObject("icons8_add_20", resourceCulture);

	internal static Bitmap icons8_attach_16 => (Bitmap)ResourceManager.GetObject("icons8_attach_16", resourceCulture);

	internal static Bitmap icons8_bill_32 => (Bitmap)ResourceManager.GetObject("icons8-bill-32", resourceCulture);

	internal static Bitmap icons8_copy_16 => (Bitmap)ResourceManager.GetObject("icons8_copy_16", resourceCulture);

	internal static Bitmap icons8_cut_16 => (Bitmap)ResourceManager.GetObject("icons8_cut_16", resourceCulture);

	internal static Bitmap icons8_delete_bin_16 => (Bitmap)ResourceManager.GetObject("icons8_delete_bin_16", resourceCulture);

	internal static Bitmap icons8_excel_48 => (Bitmap)ResourceManager.GetObject("icons8-excel-48", resourceCulture);

	internal static Bitmap icons8_help_16 => (Bitmap)ResourceManager.GetObject("icons8_help_16", resourceCulture);

	internal static Bitmap icons8_in_transit_32 => (Bitmap)ResourceManager.GetObject("icons8-in-transit-32", resourceCulture);

	internal static Bitmap icons8_item_50 => (Bitmap)ResourceManager.GetObject("icons8-item-50", resourceCulture);

	internal static Bitmap icons8_messaging_16 => (Bitmap)ResourceManager.GetObject("icons8_messaging_16", resourceCulture);

	internal static Bitmap icons8_pencil_16 => (Bitmap)ResourceManager.GetObject("icons8_pencil_16", resourceCulture);

	internal static Bitmap icons8_pending_32 => (Bitmap)ResourceManager.GetObject("icons8-pending-32", resourceCulture);

	internal static Bitmap icons8_print_16 => (Bitmap)ResourceManager.GetObject("icons8_print_16", resourceCulture);

	internal static Bitmap icons8_save_16 => (Bitmap)ResourceManager.GetObject("icons8_save_16", resourceCulture);

	internal static Bitmap icons8_search_16 => (Bitmap)ResourceManager.GetObject("icons8_search_16", resourceCulture);

	internal static Bitmap icons8_setting => (Bitmap)ResourceManager.GetObject("icons8-setting", resourceCulture);

	internal static Bitmap icons8_user_rights_30 => (Bitmap)ResourceManager.GetObject("icons8-user-rights-30", resourceCulture);

	internal static Bitmap images => (Bitmap)ResourceManager.GetObject("images", resourceCulture);

	internal static Bitmap Import => (Bitmap)ResourceManager.GetObject("Import", resourceCulture);

	internal static Bitmap info_i => (Bitmap)ResourceManager.GetObject("info-i", resourceCulture);

	internal static Bitmap Inventory_Reports => (Bitmap)ResourceManager.GetObject("Inventory Reports", resourceCulture);

	internal static Bitmap Inventry_Defination => (Bitmap)ResourceManager.GetObject("Inventry Defination", resourceCulture);

	internal static Bitmap Lab => (Bitmap)ResourceManager.GetObject("Lab", resourceCulture);

	internal static Bitmap Lab_Reports => (Bitmap)ResourceManager.GetObject("Lab Reports", resourceCulture);

	internal static Bitmap list => (Bitmap)ResourceManager.GetObject("list", resourceCulture);

	internal static Bitmap lock_line => (Bitmap)ResourceManager.GetObject("lock-line", resourceCulture);

	internal static Bitmap LoginPage => (Bitmap)ResourceManager.GetObject("LoginPage", resourceCulture);

	internal static Bitmap mail_line => (Bitmap)ResourceManager.GetObject("mail-line", resourceCulture);

	internal static Bitmap MCBBank_PK => (Bitmap)ResourceManager.GetObject("MCBBank_PK", resourceCulture);

	internal static Bitmap MeezanBank_PK => (Bitmap)ResourceManager.GetObject("MeezanBank_PK", resourceCulture);

	internal static Bitmap MessageIcon_removebg_preview => (Bitmap)ResourceManager.GetObject("MessageIcon-removebg-preview", resourceCulture);

	internal static Bitmap MessageIcon_removebg_preview__2_ => (Bitmap)ResourceManager.GetObject("MessageIcon-removebg-preview (2)", resourceCulture);

	internal static Bitmap Microsoft_Excel_Icon => (Bitmap)ResourceManager.GetObject("Microsoft_Excel_Icon", resourceCulture);

	internal static Bitmap Microsoft_Word_Logo => (Bitmap)ResourceManager.GetObject("Microsoft_Word-Logo", resourceCulture);

	internal static Bitmap NationalBankOfPakistan_NBP_PK => (Bitmap)ResourceManager.GetObject("NationalBankOfPakistan_NBP_PK", resourceCulture);

	internal static Bitmap Nib => (Bitmap)ResourceManager.GetObject("Nib", resourceCulture);

	internal static Bitmap No_Preview => (Bitmap)ResourceManager.GetObject("No_Preview", resourceCulture);

	internal static Bitmap NoItemImageFound => (Bitmap)ResourceManager.GetObject("NoItemImageFound", resourceCulture);

	internal static Bitmap OrderCardIcon => (Bitmap)ResourceManager.GetObject("OrderCardIcon", resourceCulture);

	internal static Bitmap Pdf_Logo => (Bitmap)ResourceManager.GetObject("Pdf_Logo", resourceCulture);

	internal static Bitmap phone_line => (Bitmap)ResourceManager.GetObject("phone-line", resourceCulture);

	internal static Bitmap Pinwheel => (Bitmap)ResourceManager.GetObject("Pinwheel", resourceCulture);

	internal static Bitmap play => (Bitmap)ResourceManager.GetObject("play", resourceCulture);

	internal static Bitmap Production => (Bitmap)ResourceManager.GetObject("Production", resourceCulture);

	internal static Bitmap Purchase_Trading => (Bitmap)ResourceManager.GetObject("Purchase Trading", resourceCulture);

	internal static Bitmap QuickActionCardIcon => (Bitmap)ResourceManager.GetObject("QuickActionCardIcon", resourceCulture);

	internal static Bitmap referesf => (Bitmap)ResourceManager.GetObject("referesf", resourceCulture);

	internal static Bitmap reset => (Bitmap)ResourceManager.GetObject("reset", resourceCulture);

	internal static Bitmap reset__1_ => (Bitmap)ResourceManager.GetObject("reset (1)", resourceCulture);

	internal static Bitmap ribbon => (Bitmap)ResourceManager.GetObject("ribbon", resourceCulture);

	internal static Bitmap RightsIcon => (Bitmap)ResourceManager.GetObject("RightsIcon", resourceCulture);

	internal static Bitmap Sale_Trading => (Bitmap)ResourceManager.GetObject("Sale Trading", resourceCulture);

	internal static Bitmap SaleIconCard => (Bitmap)ResourceManager.GetObject("SaleIconCard", resourceCulture);

	internal static Bitmap show => (Bitmap)ResourceManager.GetObject("show", resourceCulture);

	internal static Bitmap SilkBank_PK => (Bitmap)ResourceManager.GetObject("SilkBank_PK", resourceCulture);

	internal static Bitmap SilkBank_PK1 => (Bitmap)ResourceManager.GetObject("SilkBank_PK1", resourceCulture);

	internal static Bitmap SoneriBank_PK => (Bitmap)ResourceManager.GetObject("SoneriBank_PK", resourceCulture);

	internal static Bitmap SortAsc => (Bitmap)ResourceManager.GetObject("SortAsc", resourceCulture);

	internal static Bitmap SortDesc => (Bitmap)ResourceManager.GetObject("SortDesc", resourceCulture);

	internal static Bitmap stack_fill => (Bitmap)ResourceManager.GetObject("stack-fill", resourceCulture);

	internal static Bitmap StandardChartered => (Bitmap)ResourceManager.GetObject("StandardChartered", resourceCulture);

	internal static Bitmap star_fill => (Bitmap)ResourceManager.GetObject("star-fill", resourceCulture);

	internal static Bitmap star_line => (Bitmap)ResourceManager.GetObject("star-line", resourceCulture);

	internal static Bitmap Stock_Reports => (Bitmap)ResourceManager.GetObject("Stock Reports", resourceCulture);

	internal static Bitmap Store_Management => (Bitmap)ResourceManager.GetObject("Store Management", resourceCulture);

	internal static Bitmap Store_Management1 => (Bitmap)ResourceManager.GetObject("Store Management1", resourceCulture);

	internal static Bitmap Supplier_Purchase => (Bitmap)ResourceManager.GetObject("Supplier Purchase", resourceCulture);

	internal static Bitmap Taxation => (Bitmap)ResourceManager.GetObject("Taxation", resourceCulture);

	internal static Bitmap TopPacksCardIcon => (Bitmap)ResourceManager.GetObject("TopPacksCardIcon", resourceCulture);

	internal static Bitmap TopTradedItemsCardIcon => (Bitmap)ResourceManager.GetObject("TopTradedItemsCardIcon", resourceCulture);

	internal static Bitmap txtFile_Logo => (Bitmap)ResourceManager.GetObject("txtFile_Logo", resourceCulture);

	internal static Bitmap UnitedBank_UBL_PK => (Bitmap)ResourceManager.GetObject("UnitedBank_UBL_PK", resourceCulture);

	internal static Bitmap UpdateIcon => (Bitmap)ResourceManager.GetObject("UpdateIcon", resourceCulture);

	internal static Bitmap user_line => (Bitmap)ResourceManager.GetObject("user-line", resourceCulture);

	internal static Bitmap Warning => (Bitmap)ResourceManager.GetObject("Warning", resourceCulture);

	internal static Bitmap WhiteStar => (Bitmap)ResourceManager.GetObject("WhiteStar", resourceCulture);

	internal static Bitmap Zip_Logo => (Bitmap)ResourceManager.GetObject("Zip_Logo", resourceCulture);

	internal Resources()
	{
	}
}

}
