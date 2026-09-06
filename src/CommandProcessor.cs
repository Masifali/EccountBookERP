using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
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

internal class CommandProcessor
{
	private ConcurrentQueue<Command> _commandQueue = new ConcurrentQueue<Command>();

	private bool _running;

	private Task _task;

	public void Start()
	{
		_running = true;
		_task = Task.Run(delegate
		{
			while (_running)
			{
				Thread.Sleep(1);
				Command result = null;
				_commandQueue.TryDequeue(out result);
				if (result != null && !result.Execute())
				{
					Thread.Sleep(500);
					_commandQueue.Enqueue(result);
				}
			}
		});
	}

	public void Stop()
	{
		_running = false;
		try
		{
			_task.Wait();
		}
		catch (AggregateException)
		{
		}
		_task.Dispose();
	}

	public void PostCommand(Command command)
	{
		_commandQueue.Enqueue(command);
	}
}

}
