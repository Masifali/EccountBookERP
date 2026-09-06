using System;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace CameraControl
{

public class CameraEvent
{
	public enum Type
	{
		NONE,
		ERROR,
		DEVICE_BUSY,
		DOWNLOAD_START,
		DOWNLOAD_COMPLETE,
		EVFDATA_CHANGED,
		PROGRESS_REPORT,
		PROPERTY_CHANGED,
		PROPERTY_DESC_CHANGED,
		DELETE_START,
		DELETE_COMPLETE,
		PROGRESS,
		ANGLEINFO,
		MOUSE_CURSOR,
		SHUT_DOWN
	}

	private Type _type;

	private IntPtr _arg;

	public CameraEvent(Type type, IntPtr arg)
	{
		_type = type;
		_arg = arg;
	}

	public Type GetEventType()
	{
		return _type;
	}

	public IntPtr GetArg()
	{
		return _arg;
	}
}

}
