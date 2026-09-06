using System;
using System.Runtime.InteropServices;
using EDSDKLib;
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

internal class CameraEventListener
{
	public static uint HandleObjectEvent(uint inEvent, IntPtr inRef, IntPtr inContext)
	{
		CameraController controller = (CameraController)GCHandle.FromIntPtr(inContext).Target;
		if (inEvent == 520)
		{
			FireEvent(ref controller, ActionEvent.Command.DOWNLOAD, inRef);
		}
		else if (inRef != IntPtr.Zero)
		{
			EDSDK.EdsRelease(inRef);
		}
		return 0u;
	}

	public static uint HandlePropertyEvent(uint inEvent, uint inPropertyID, uint inParam, IntPtr inContext)
	{
		CameraController controller = (CameraController)GCHandle.FromIntPtr(inContext).Target;
		switch (inEvent)
		{
		case 257u:
			FireEvent(ref controller, ActionEvent.Command.GET_PROPERTY, (IntPtr)inPropertyID);
			break;
		case 258u:
			FireEvent(ref controller, ActionEvent.Command.GET_PROPERTYDESC, (IntPtr)inPropertyID);
			break;
		}
		return 0u;
	}

	public static uint HandleStateEvent(uint inEvent, uint inParameter, IntPtr inContext)
	{
		CameraController controller = (CameraController)GCHandle.FromIntPtr(inContext).Target;
		if (inEvent == 769)
		{
			FireEvent(ref controller, ActionEvent.Command.SHUT_DOWN, IntPtr.Zero);
		}
		return 0u;
	}

	private static void FireEvent(ref CameraController controller, ActionEvent.Command command, IntPtr arg)
	{
		ActionEvent e = new ActionEvent(command, arg);
		controller.ActionPerformed(e);
	}
}

}
