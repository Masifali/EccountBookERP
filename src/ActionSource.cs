using System;
using System.Collections.Generic;
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

public class ActionSource
{
	private List<ActionListener> listeners = new List<ActionListener>();

	public void AddActionListener(ref ActionListener listener)
	{
		listeners.Add(listener);
	}

	public void RemoveActionListener(ref ActionListener listener)
	{
		listeners.Remove(listener);
	}

	public void FireEvent(ActionEvent.Command command, IntPtr arg)
	{
		ActionEvent e = new ActionEvent(command, arg);
		for (int i = 0; i < listeners.Count; i++)
		{
			listeners[i].ActionPerformed(e);
		}
	}
}

}
