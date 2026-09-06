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

public abstract class Observable
{
	private List<IObserver> observers = new List<IObserver>();

	public void Add(ref IObserver observer)
	{
		observers.Add(observer);
	}

	public void Remove(ref IObserver observer)
	{
		observers.Remove(observer);
	}

	public void NotifyObservers(CameraEvent e)
	{
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].Update(this, e);
		}
	}
}

}
